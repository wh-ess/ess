///#source 1 1 /Scripts/svg/svg.js
/* svg.js 1.1.0 - svg selector inventor polyfill regex default color array pointarray patharray number viewbox bbox rbox element parent container fx relative event defs group arrange mask clip gradient pattern doc shape symbol use rect ellipse line poly path image text textpath nested hyperlink marker sugar set data memory helpers - svgjs.com/license */
;(function(root, factory) {
  if (typeof define === 'function' && define.amd) {
    define(factory);
  } else if (typeof exports === 'object') {
    module.exports = factory();
  } else {
    root.SVG = factory();
  }
}(this, function() {

  var SVG = this.SVG = function(element) {
    if (SVG.supported) {
      element = new SVG.Doc(element)
  
      if (!SVG.parser)
        SVG.prepare(element)
  
      return element
    }
  }
  
  // Default namespaces
  SVG.ns    = 'http://www.w3.org/2000/svg'
  SVG.xmlns = 'http://www.w3.org/2000/xmlns/'
  SVG.xlink = 'http://www.w3.org/1999/xlink'
  
  // Element id sequence
  SVG.did  = 1000
  
  // Get next named element id
  SVG.eid = function(name) {
    return 'Svgjs' + name.charAt(0).toUpperCase() + name.slice(1) + (SVG.did++)
  }
  
  // Method for element creation
  SVG.create = function(name) {
    /* create element */
    var element = document.createElementNS(this.ns, name)
    
    /* apply unique id */
    element.setAttribute('id', this.eid(name))
    
    return element
  }
  
  // Method for extending objects
  SVG.extend = function() {
    var modules, methods, key, i
    
    /* get list of modules */
    modules = [].slice.call(arguments)
    
    /* get object with extensions */
    methods = modules.pop()
    
    for (i = modules.length - 1; i >= 0; i--)
      if (modules[i])
        for (key in methods)
          modules[i].prototype[key] = methods[key]
  
    /* make sure SVG.Set inherits any newly added methods */
    if (SVG.Set && SVG.Set.inherit)
      SVG.Set.inherit()
  }
  
  // Initialize parsing element
  SVG.prepare = function(element) {
    /* select document body and create invisible svg element */
    var body = document.getElementsByTagName('body')[0]
      , draw = (body ? new SVG.Doc(body) : element.nested()).size(2, 0)
      , path = SVG.create('path')
  
    /* insert parsers */
    draw.node.appendChild(path)
  
    /* create parser object */
    SVG.parser = {
      body: body || element.parent
    , draw: draw.style('opacity:0;position:fixed;left:100%;top:100%;overflow:hidden')
    , poly: draw.polyline().node
    , path: path
    }
  }
  
  // svg support test
  SVG.supported = (function() {
    return !! document.createElementNS &&
           !! document.createElementNS(SVG.ns,'svg').createSVGRect
  })()
  
  if (!SVG.supported) return false


  SVG.get = function(id) {
    var node = document.getElementById(idFromReference(id) || id)
    if (node) return node.instance
  }

  SVG.invent = function(config) {
  	/* create element initializer */
  	var initializer = typeof config.create == 'function' ?
  		config.create :
  		function() {
  			this.constructor.call(this, SVG.create(config.create))
  		}
  
  	/* inherit prototype */
  	if (config.inherit)
  		initializer.prototype = new config.inherit
  
  	/* extend with methods */
  	if (config.extend)
  		SVG.extend(initializer, config.extend)
  
  	/* attach construct method to parent */
  	if (config.construct)
  		SVG.extend(config.parent || SVG.Container, config.construct)
  
  	return initializer
  }

  if (typeof CustomEvent !== 'function') {
    // Code from: https://developer.mozilla.org/en-US/docs/Web/API/CustomEvent
    var CustomEvent = function(event, options) {
      options = options || { bubbles: false, cancelable: false, detail: undefined }
      var e = document.createEvent('CustomEvent')
      e.initCustomEvent(event, options.bubbles, options.cancelable, options.detail)
      return e
    }
  
    CustomEvent.prototype = window.Event.prototype
  
    window.CustomEvent = CustomEvent
  }
  
  // requestAnimationFrame / cancelAnimationFrame Polyfill with fallback based on Paul Irish
  (function(w) {
    var lastTime = 0
    var vendors = ['moz', 'webkit']
    
    for(var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
      w.requestAnimationFrame = w[vendors[x] + 'RequestAnimationFrame']
      w.cancelAnimationFrame  = w[vendors[x] + 'CancelAnimationFrame'] ||
                                w[vendors[x] + 'CancelRequestAnimationFrame']
    }
   
    w.requestAnimationFrame = w.requestAnimationFrame || 
      function(callback) {
        var currTime = new Date().getTime()
        var timeToCall = Math.max(0, 16 - (currTime - lastTime))
        
        var id = w.setTimeout(function() {
          callback(currTime + timeToCall)
        }, timeToCall)
        
        lastTime = currTime + timeToCall
        return id
      }
   
    w.cancelAnimationFrame = w.cancelAnimationFrame || w.clearTimeout;
  
  }(window))

  SVG.regex = {
    /* parse unit value */
    unit:         /^(-?[\d\.]+)([a-z%]{0,2})$/
    
    /* parse hex value */
  , hex:          /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i
    
    /* parse rgb value */
  , rgb:          /rgb\((\d+),(\d+),(\d+)\)/
    
    /* parse reference id */
  , reference:    /#([a-z0-9\-_]+)/i
  
    /* test hex value */
  , isHex:        /^#[a-f0-9]{3,6}$/i
    
    /* test rgb value */
  , isRgb:        /^rgb\(/
    
    /* test css declaration */
  , isCss:        /[^:]+:[^;]+;?/
    
    /* test for blank string */
  , isBlank:      /^(\s+)?$/
    
    /* test for numeric string */
  , isNumber:     /^-?[\d\.]+$/
  
    /* test for percent value */
  , isPercent:    /^-?[\d\.]+%$/
  
    /* test for image url */
  , isImage:      /\.(jpg|jpeg|png|gif)(\?[^=]+.*)?/i
    
    /* test for namespaced event */
  , isEvent:      /^[\w]+:[\w]+$/
  
  }

  SVG.defaults = {
    // Default matrix
    matrix:       '1 0 0 1 0 0'
    
    // Default attribute values
  , attrs: {
      /* fill and stroke */
      'fill-opacity':     1
    , 'stroke-opacity':   1
    , 'stroke-width':     0
    , 'stroke-linejoin':  'miter'
    , 'stroke-linecap':   'butt'
    , fill:               '#000000'
    , stroke:             '#000000'
    , opacity:            1
      /* position */
    , x:                  0
    , y:                  0
    , cx:                 0
    , cy:                 0
      /* size */  
    , width:              0
    , height:             0
      /* radius */  
    , r:                  0
    , rx:                 0
    , ry:                 0
      /* gradient */  
    , offset:             0
    , 'stop-opacity':     1
    , 'stop-color':       '#000000'
      /* text */
    , 'font-size':        16
    , 'font-family':      'Helvetica, Arial, sans-serif'
    , 'text-anchor':      'start'
    }
    
    // Default transformation values
  , trans: function() {
      return {
        /* translate */
        x:        0
      , y:        0
        /* scale */
      , scaleX:   1
      , scaleY:   1
        /* rotate */
      , rotation: 0
        /* skew */
      , skewX:    0
      , skewY:    0
        /* matrix */
      , matrix:   this.matrix
      , a:        1
      , b:        0
      , c:        0
      , d:        1
      , e:        0
      , f:        0
      }
    }
    
  }

  SVG.Color = function(color) {
    var match
    
    /* initialize defaults */
    this.r = 0
    this.g = 0
    this.b = 0
    
    /* parse color */
    if (typeof color === 'string') {
      if (SVG.regex.isRgb.test(color)) {
        /* get rgb values */
        match = SVG.regex.rgb.exec(color.replace(/\s/g,''))
        
        /* parse numeric values */
        this.r = parseInt(match[1])
        this.g = parseInt(match[2])
        this.b = parseInt(match[3])
        
      } else if (SVG.regex.isHex.test(color)) {
        /* get hex values */
        match = SVG.regex.hex.exec(fullHex(color))
  
        /* parse numeric values */
        this.r = parseInt(match[1], 16)
        this.g = parseInt(match[2], 16)
        this.b = parseInt(match[3], 16)
  
      }
      
    } else if (typeof color === 'object') {
      this.r = color.r
      this.g = color.g
      this.b = color.b
      
    }
      
  }
  
  SVG.extend(SVG.Color, {
    // Default to hex conversion
    toString: function() {
      return this.toHex()
    }
    // Build hex value
  , toHex: function() {
      return '#'
        + compToHex(this.r)
        + compToHex(this.g)
        + compToHex(this.b)
    }
    // Build rgb value
  , toRgb: function() {
      return 'rgb(' + [this.r, this.g, this.b].join() + ')'
    }
    // Calculate true brightness
  , brightness: function() {
      return (this.r / 255 * 0.30)
           + (this.g / 255 * 0.59)
           + (this.b / 255 * 0.11)
    }
    // Make color morphable
  , morph: function(color) {
      this.destination = new SVG.Color(color)
  
      return this
    }
    // Get morphed color at given position
  , at: function(pos) {
      /* make sure a destination is defined */
      if (!this.destination) return this
  
      /* normalise pos */
      pos = pos < 0 ? 0 : pos > 1 ? 1 : pos
  
      /* generate morphed color */
      return new SVG.Color({
        r: ~~(this.r + (this.destination.r - this.r) * pos)
      , g: ~~(this.g + (this.destination.g - this.g) * pos)
      , b: ~~(this.b + (this.destination.b - this.b) * pos)
      })
    }
    
  })
  
  // Testers
  
  // Test if given value is a color string
  SVG.Color.test = function(color) {
    color += ''
    return SVG.regex.isHex.test(color)
        || SVG.regex.isRgb.test(color)
  }
  
  // Test if given value is a rgb object
  SVG.Color.isRgb = function(color) {
    return color && typeof color.r == 'number'
                 && typeof color.g == 'number'
                 && typeof color.b == 'number'
  }
  
  // Test if given value is a color
  SVG.Color.isColor = function(color) {
    return SVG.Color.isRgb(color) || SVG.Color.test(color)
  }

  SVG.Array = function(array, fallback) {
    array = (array || []).valueOf()
  
    /* if array is empty and fallback is provided, use fallback */
    if (array.length == 0 && fallback)
      array = fallback.valueOf()
  
    /* parse array */
    this.value = this.parse(array)
  }
  
  SVG.extend(SVG.Array, {
    // Make array morphable
    morph: function(array) {
      this.destination = this.parse(array)
  
      /* normalize length of arrays */
      if (this.value.length != this.destination.length) {
        var lastValue       = this.value[this.value.length - 1]
          , lastDestination = this.destination[this.destination.length - 1]
  
        while(this.value.length > this.destination.length)
          this.destination.push(lastDestination)
        while(this.value.length < this.destination.length)
          this.value.push(lastValue)
      }
  
      return this
    }
    // Clean up any duplicate points
  , settle: function() {
      /* find all unique values */
      for (var i = 0, il = this.value.length, seen = []; i < il; i++)
        if (seen.indexOf(this.value[i]) == -1)
          seen.push(this.value[i])
  
      /* set new value */
      return this.value = seen
    }
    // Get morphed array at given position
  , at: function(pos) {
      /* make sure a destination is defined */
      if (!this.destination) return this
  
      /* generate morphed array */
      for (var i = 0, il = this.value.length, array = []; i < il; i++)
        array.push(this.value[i] + (this.destination[i] - this.value[i]) * pos)
  
      return new SVG.Array(array)
    }
    // Convert array to string
  , toString: function() {
      return this.value.join(' ')
    }
    // Real value
  , valueOf: function() {
      return this.value
    }
    // Parse whitespace separated string
  , parse: function(array) {
      array = array.valueOf()
  
      /* if already is an array, no need to parse it */
      if (Array.isArray(array)) return array
  
      return this.split(array)
    }
    // Strip unnecessary whitespace
  , split: function(string) {
      return string.replace(/\s+/g, ' ').replace(/^\s+|\s+$/g,'').split(' ') 
    }
    // Reverse array
  , reverse: function() {
      this.value.reverse()
  
      return this
    }
  
  })
  


  SVG.PointArray = function() {
    this.constructor.apply(this, arguments)
  }
  
  // Inherit from SVG.Array
  SVG.PointArray.prototype = new SVG.Array
  
  SVG.extend(SVG.PointArray, {
    // Convert array to string
    toString: function() {
      /* convert to a poly point string */
      for (var i = 0, il = this.value.length, array = []; i < il; i++)
        array.push(this.value[i].join(','))
  
      return array.join(' ')
    }
    // Get morphed array at given position
  , at: function(pos) {
      /* make sure a destination is defined */
      if (!this.destination) return this
  
      /* generate morphed point string */
      for (var i = 0, il = this.value.length, array = []; i < il; i++)
        array.push([
          this.value[i][0] + (this.destination[i][0] - this.value[i][0]) * pos
        , this.value[i][1] + (this.destination[i][1] - this.value[i][1]) * pos
        ])
  
      return new SVG.PointArray(array)
    }
    // Parse point string
  , parse: function(array) {
      array = array.valueOf()
  
      /* if already is an array, no need to parse it */
      if (Array.isArray(array)) return array
  
      /* split points */
      array = this.split(array)
  
      /* parse points */
      for (var i = 0, il = array.length, p, points = []; i < il; i++) {
        p = array[i].split(',')
        points.push([parseFloat(p[0]), parseFloat(p[1])])
      }
  
      return points
    }
    // Move point string
  , move: function(x, y) {
      var box = this.bbox()
  
      /* get relative offset */
      x -= box.x
      y -= box.y
  
      /* move every point */
      if (!isNaN(x) && !isNaN(y))
        for (var i = this.value.length - 1; i >= 0; i--)
          this.value[i] = [this.value[i][0] + x, this.value[i][1] + y]
  
      return this
    }
    // Resize poly string
  , size: function(width, height) {
      var i, box = this.bbox()
  
      /* recalculate position of all points according to new size */
      for (i = this.value.length - 1; i >= 0; i--) {
        this.value[i][0] = ((this.value[i][0] - box.x) * width)  / box.width  + box.x
        this.value[i][1] = ((this.value[i][1] - box.y) * height) / box.height + box.y
      }
  
      return this
    }
    // Get bounding box of points
  , bbox: function() {
      SVG.parser.poly.setAttribute('points', this.toString())
  
      return SVG.parser.poly.getBBox()
    }
  
  })

  SVG.PathArray = function(array, fallback) {
    this.constructor.call(this, array, fallback)
  }
  
  // Inherit from SVG.Array
  SVG.PathArray.prototype = new SVG.Array
  
  SVG.extend(SVG.PathArray, {
    // Convert array to string
    toString: function() {
      return arrayToString(this.value)
    }
    // Move path string
  , move: function(x, y) {
  		/* get bounding box of current situation */
  		var box = this.bbox()
  		
      /* get relative offset */
      x -= box.x
      y -= box.y
  
      if (!isNaN(x) && !isNaN(y)) {
        /* move every point */
        for (var l, i = this.value.length - 1; i >= 0; i--) {
          l = this.value[i][0]
  
          if (l == 'M' || l == 'L' || l == 'T')  {
            this.value[i][1] += x
            this.value[i][2] += y
  
          } else if (l == 'H')  {
            this.value[i][1] += x
  
          } else if (l == 'V')  {
            this.value[i][1] += y
  
          } else if (l == 'C' || l == 'S' || l == 'Q')  {
            this.value[i][1] += x
            this.value[i][2] += y
            this.value[i][3] += x
            this.value[i][4] += y
  
            if (l == 'C')  {
              this.value[i][5] += x
              this.value[i][6] += y
            }
  
          } else if (l == 'A')  {
            this.value[i][6] += x
            this.value[i][7] += y
          }
  
        }
      }
  
      return this
    }
    // Resize path string
  , size: function(width, height) {
  		/* get bounding box of current situation */
  		var i, l, box = this.bbox()
  
      /* recalculate position of all points according to new size */
      for (i = this.value.length - 1; i >= 0; i--) {
        l = this.value[i][0]
  
        if (l == 'M' || l == 'L' || l == 'T')  {
          this.value[i][1] = ((this.value[i][1] - box.x) * width)  / box.width  + box.x
          this.value[i][2] = ((this.value[i][2] - box.y) * height) / box.height + box.y
  
        } else if (l == 'H')  {
          this.value[i][1] = ((this.value[i][1] - box.x) * width)  / box.width  + box.x
  
        } else if (l == 'V')  {
          this.value[i][1] = ((this.value[i][1] - box.y) * height) / box.height + box.y
  
        } else if (l == 'C' || l == 'S' || l == 'Q')  {
          this.value[i][1] = ((this.value[i][1] - box.x) * width)  / box.width  + box.x
          this.value[i][2] = ((this.value[i][2] - box.y) * height) / box.height + box.y
          this.value[i][3] = ((this.value[i][3] - box.x) * width)  / box.width  + box.x
          this.value[i][4] = ((this.value[i][4] - box.y) * height) / box.height + box.y
  
          if (l == 'C')  {
            this.value[i][5] = ((this.value[i][5] - box.x) * width)  / box.width  + box.x
            this.value[i][6] = ((this.value[i][6] - box.y) * height) / box.height + box.y
          }
  
        } else if (l == 'A')  {
          /* resize radii */
          this.value[i][1] = (this.value[i][1] * width)  / box.width
          this.value[i][2] = (this.value[i][2] * height) / box.height
  
          /* move position values */
          this.value[i][6] = ((this.value[i][6] - box.x) * width)  / box.width  + box.x
          this.value[i][7] = ((this.value[i][7] - box.y) * height) / box.height + box.y
        }
  
      }
  
      return this
    }
    // Absolutize and parse path to array
  , parse: function(array) {
      /* if it's already is a patharray, no need to parse it */
      if (array instanceof SVG.PathArray) return array.valueOf()
  
      /* prepare for parsing */
      var i, il, x0, y0, x1, y1, x2, y2, s, seg, segs
        , x = 0
        , y = 0
      
      /* populate working path */
      SVG.parser.path.setAttribute('d', typeof array === 'string' ? array : arrayToString(array))
      
      /* get segments */
      segs = SVG.parser.path.pathSegList
  
      for (i = 0, il = segs.numberOfItems; i < il; ++i) {
        seg = segs.getItem(i)
        s = seg.pathSegTypeAsLetter
  
        /* yes, this IS quite verbose but also about 30 times faster than .test() with a precompiled regex */
        if (s == 'M' || s == 'L' || s == 'H' || s == 'V' || s == 'C' || s == 'S' || s == 'Q' || s == 'T' || s == 'A') {
          if ('x' in seg) x = seg.x
          if ('y' in seg) y = seg.y
  
        } else {
          if ('x1' in seg) x1 = x + seg.x1
          if ('x2' in seg) x2 = x + seg.x2
          if ('y1' in seg) y1 = y + seg.y1
          if ('y2' in seg) y2 = y + seg.y2
          if ('x'  in seg) x += seg.x
          if ('y'  in seg) y += seg.y
  
          if (s == 'm')
            segs.replaceItem(SVG.parser.path.createSVGPathSegMovetoAbs(x, y), i)
          else if (s == 'l')
            segs.replaceItem(SVG.parser.path.createSVGPathSegLinetoAbs(x, y), i)
          else if (s == 'h')
            segs.replaceItem(SVG.parser.path.createSVGPathSegLinetoHorizontalAbs(x), i)
          else if (s == 'v')
            segs.replaceItem(SVG.parser.path.createSVGPathSegLinetoVerticalAbs(y), i)
          else if (s == 'c')
            segs.replaceItem(SVG.parser.path.createSVGPathSegCurvetoCubicAbs(x, y, x1, y1, x2, y2), i)
          else if (s == 's')
            segs.replaceItem(SVG.parser.path.createSVGPathSegCurvetoCubicSmoothAbs(x, y, x2, y2), i)
          else if (s == 'q')
            segs.replaceItem(SVG.parser.path.createSVGPathSegCurvetoQuadraticAbs(x, y, x1, y1), i)
          else if (s == 't')
            segs.replaceItem(SVG.parser.path.createSVGPathSegCurvetoQuadraticSmoothAbs(x, y), i)
          else if (s == 'a')
            segs.replaceItem(SVG.parser.path.createSVGPathSegArcAbs(x, y, seg.r1, seg.r2, seg.angle, seg.largeArcFlag, seg.sweepFlag), i)
          else if (s == 'z' || s == 'Z') {
            x = x0
            y = y0
          }
        }
  
        /* record the start of a subpath */
        if (s == 'M' || s == 'm') {
          x0 = x
          y0 = y
        }
      }
  
      /* build internal representation */
      array = []
      segs  = SVG.parser.path.pathSegList
      
      for (i = 0, il = segs.numberOfItems; i < il; ++i) {
        seg = segs.getItem(i)
        s = seg.pathSegTypeAsLetter
        x = [s]
  
        if (s == 'M' || s == 'L' || s == 'T')
          x.push(seg.x, seg.y)
        else if (s == 'H')
          x.push(seg.x)
        else if (s == 'V')
          x.push(seg.y)
        else if (s == 'C')
          x.push(seg.x1, seg.y1, seg.x2, seg.y2, seg.x, seg.y)
        else if (s == 'S')
          x.push(seg.x2, seg.y2, seg.x, seg.y)
        else if (s == 'Q')
          x.push(seg.x1, seg.y1, seg.x, seg.y)
        else if (s == 'A')
          x.push(seg.r1, seg.r2, seg.angle, seg.largeArcFlag|0, seg.sweepFlag|0, seg.x, seg.y)
  
        /* store segment */
        array.push(x)
      }
      
      return array
    }
    // Get bounding box of path
  , bbox: function() {
      SVG.parser.path.setAttribute('d', this.toString())
  
      return SVG.parser.path.getBBox()
    }
  
  })

  SVG.Number = function(value) {
  
    /* initialize defaults */
    this.value = 0
    this.unit = ''
  
    /* parse value */
    if (typeof value === 'number') {
      /* ensure a valid numeric value */
      this.value = isNaN(value) ? 0 : !isFinite(value) ? (value < 0 ? -3.4e+38 : +3.4e+38) : value
  
    } else if (typeof value === 'string') {
      var match = value.match(SVG.regex.unit)
  
      if (match) {
        /* make value numeric */
        this.value = parseFloat(match[1])
      
        /* normalize percent value */
        if (match[2] == '%')
          this.value /= 100
        else if (match[2] == 's')
          this.value *= 1000
      
        /* store unit */
        this.unit = match[2]
      }
  
    } else {
      if (value instanceof SVG.Number) {
        this.value = value.value
        this.unit  = value.unit
      }
    }
  
  }
  
  SVG.extend(SVG.Number, {
    // Stringalize
    toString: function() {
      return (
        this.unit == '%' ?
          ~~(this.value * 1e8) / 1e6:
        this.unit == 's' ?
          this.value / 1e3 :
          this.value
      ) + this.unit
    }
  , // Convert to primitive
    valueOf: function() {
      return this.value
    }
    // Add number
  , plus: function(number) {
      this.value = this + new SVG.Number(number)
  
      return this
    }
    // Subtract number
  , minus: function(number) {
      return this.plus(-new SVG.Number(number))
    }
    // Multiply number
  , times: function(number) {
      this.value = this * new SVG.Number(number)
  
      return this
    }
    // Divide number
  , divide: function(number) {
      this.value = this / new SVG.Number(number)
  
      return this
    }
    // Convert to different unit
  , to: function(unit) {
      if (typeof unit === 'string')
        this.unit = unit
  
      return this
    }
    // Make number morphable
  , morph: function(number) {
      this.destination = new SVG.Number(number)
  
      return this
    }
    // Get morphed number at given position
  , at: function(pos) {
      /* make sure a destination is defined */
      if (!this.destination) return this
  
      /* generate new morphed number */
      return new SVG.Number(this.destination)
          .minus(this)
          .times(pos)
          .plus(this)
    }
  
  })

  SVG.ViewBox = function(element) {
    var x, y, width, height
      , wm   = 1 /* width multiplier */
      , hm   = 1 /* height multiplier */
      , box  = element.bbox()
      , view = (element.attr('viewBox') || '').match(/-?[\d\.]+/g)
      , we   = element
      , he   = element
  
    /* get dimensions of current node */
    width  = new SVG.Number(element.width())
    height = new SVG.Number(element.height())
  
    /* find nearest non-percentual dimensions */
    while (width.unit == '%') {
      wm *= width.value
      width = new SVG.Number(we instanceof SVG.Doc ? we.parent.offsetWidth : we.parent.width())
      we = we.parent
    }
    while (height.unit == '%') {
      hm *= height.value
      height = new SVG.Number(he instanceof SVG.Doc ? he.parent.offsetHeight : he.parent.height())
      he = he.parent
    }
    
    /* ensure defaults */
    this.x      = box.x
    this.y      = box.y
    this.width  = width  * wm
    this.height = height * hm
    this.zoom   = 1
    
    if (view) {
      /* get width and height from viewbox */
      x      = parseFloat(view[0])
      y      = parseFloat(view[1])
      width  = parseFloat(view[2])
      height = parseFloat(view[3])
      
      /* calculate zoom accoring to viewbox */
      this.zoom = ((this.width / this.height) > (width / height)) ?
        this.height / height :
        this.width  / width
  
      /* calculate real pixel dimensions on parent SVG.Doc element */
      this.x      = x
      this.y      = y
      this.width  = width
      this.height = height
      
    }
    
  }
  
  //
  SVG.extend(SVG.ViewBox, {
    // Parse viewbox to string
    toString: function() {
      return this.x + ' ' + this.y + ' ' + this.width + ' ' + this.height
    }
    
  })

  SVG.BBox = function(element) {
    var box
  
    /* initialize zero box */
    this.x      = 0
    this.y      = 0
    this.width  = 0
    this.height = 0
    
    /* get values if element is given */
    if (element) {
      try {
        /* actual, native bounding box */
        box = element.node.getBBox()
      } catch(e) {
        /* fallback for some browsers */
        box = {
          x:      element.node.clientLeft
        , y:      element.node.clientTop
        , width:  element.node.clientWidth
        , height: element.node.clientHeight
        }
      }
      
      /* include translations on x an y */
      this.x = box.x + element.trans.x
      this.y = box.y + element.trans.y
      
      /* plain width and height */
      this.width  = box.width  * element.trans.scaleX
      this.height = box.height * element.trans.scaleY
    }
  
    /* add center, right and bottom */
    boxProperties(this)
    
  }
  
  //
  SVG.extend(SVG.BBox, {
    // merge bounding box with another, return a new instance
    merge: function(box) {
      var b = new SVG.BBox()
  
      /* merge box */
      b.x      = Math.min(this.x, box.x)
      b.y      = Math.min(this.y, box.y)
      b.width  = Math.max(this.x + this.width,  box.x + box.width)  - b.x
      b.height = Math.max(this.y + this.height, box.y + box.height) - b.y
  
      /* add center, right and bottom */
      boxProperties(b)
  
      return b
    }
  
  })

  SVG.RBox = function(element) {
    var e, zoom
      , box = {}
  
    /* initialize zero box */
    this.x      = 0
    this.y      = 0
    this.width  = 0
    this.height = 0
    
    if (element) {
      e = element.doc().parent
      zoom = element.doc().viewbox().zoom
      
      /* actual, native bounding box */
      box = element.node.getBoundingClientRect()
      
      /* get screen offset */
      this.x = box.left
      this.y = box.top
      
      /* subtract parent offset */
      this.x -= e.offsetLeft
      this.y -= e.offsetTop
      
      while (e = e.offsetParent) {
        this.x -= e.offsetLeft
        this.y -= e.offsetTop
      }
      
      /* calculate cumulative zoom from svg documents */
      e = element
      while (e = e.parent) {
        if (e.type == 'svg' && e.viewbox) {
          zoom *= e.viewbox().zoom
          this.x -= e.x() || 0
          this.y -= e.y() || 0
        }
      }
    }
    
    /* recalculate viewbox distortion */
    this.x /= zoom
    this.y /= zoom
    this.width  = box.width  /= zoom
    this.height = box.height /= zoom
    
    /* offset by window scroll position, because getBoundingClientRect changes when window is scrolled */
    this.x += typeof window.scrollX === 'number' ? window.scrollX : window.pageXOffset
    this.y += typeof window.scrollY === 'number' ? window.scrollY : window.pageYOffset
  
    /* add center, right and bottom */
    boxProperties(this)
    
  }
  
  //
  SVG.extend(SVG.RBox, {
    // merge rect box with another, return a new instance
    merge: function(box) {
      var b = new SVG.RBox()
  
      /* merge box */
      b.x      = Math.min(this.x, box.x)
      b.y      = Math.min(this.y, box.y)
      b.width  = Math.max(this.x + this.width,  box.x + box.width)  - b.x
      b.height = Math.max(this.y + this.height, box.y + box.height) - b.y
  
      /* add center, right and bottom */
      boxProperties(b)
  
      return b
    }
  
  })


  SVG.Element = SVG.invent({
    // Initialize node
    create: function(node) {
      /* make stroke value accessible dynamically */
      this._stroke = SVG.defaults.attrs.stroke
  
      /* initialize transformation store with defaults */
      this.trans = SVG.defaults.trans()
      
      /* create circular reference */
      if (this.node = node) {
        this.type = node.nodeName
        this.node.instance = this
      }
    }
  
    // Add class methods
  , extend: {
      // Move over x-axis
      x: function(x) {
        if (x != null) {
          x = new SVG.Number(x)
          x.value /= this.trans.scaleX
        }
        return this.attr('x', x)
      }
      // Move over y-axis
    , y: function(y) {
        if (y != null) {
          y = new SVG.Number(y)
          y.value /= this.trans.scaleY
        }
        return this.attr('y', y)
      }
      // Move by center over x-axis
    , cx: function(x) {
        return x == null ? this.x() + this.width() / 2 : this.x(x - this.width() / 2)
      }
      // Move by center over y-axis
    , cy: function(y) {
        return y == null ? this.y() + this.height() / 2 : this.y(y - this.height() / 2)
      }
      // Move element to given x and y values
    , move: function(x, y) {
        return this.x(x).y(y)
      }
      // Move element by its center
    , center: function(x, y) {
        return this.cx(x).cy(y)
      }
      // Set width of element
    , width: function(width) {
        return this.attr('width', width)
      }
      // Set height of element
    , height: function(height) {
        return this.attr('height', height)
      }
      // Set element size to given width and height
    , size: function(width, height) {
        var p = proportionalSize(this.bbox(), width, height)
  
        return this
          .width(new SVG.Number(p.width))
          .height(new SVG.Number(p.height))
      }
      // Clone element
    , clone: function() {
        var clone , attr
          , type = this.type
        
        /* invoke shape method with shape-specific arguments */
        clone = type == 'rect' || type == 'ellipse' ?
          this.parent[type](0,0) :
        type == 'line' ?
          this.parent[type](0,0,0,0) :
        type == 'image' ?
          this.parent[type](this.src) :
        type == 'text' ?
          this.parent[type](this.content) :
        type == 'path' ?
          this.parent[type](this.attr('d')) :
        type == 'polyline' || type == 'polygon' ?
          this.parent[type](this.attr('points')) :
        type == 'g' ?
          this.parent.group() :
          this.parent[type]()
        
        /* apply attributes attributes */
        attr = this.attr()
        delete attr.id
        clone.attr(attr)
        
        /* copy transformations */
        clone.trans = this.trans
        
        /* apply attributes and translations */
        return clone.transform({})
      }
      // Remove element
    , remove: function() {
        if (this.parent)
          this.parent.removeElement(this)
        
        return this
      }
      // Replace element
    , replace: function(element) {
        this.after(element).remove()
  
        return element
      }
      // Add element to given container and return self
    , addTo: function(parent) {
        return parent.put(this)
      }
      // Add element to given container and return container
    , putIn: function(parent) {
        return parent.add(this)
      }
      // Get parent document
    , doc: function(type) {
        return this._parent(type || SVG.Doc)
      }
      // Set svg element attribute
    , attr: function(a, v, n) {
        if (a == null) {
          /* get an object of attributes */
          a = {}
          v = this.node.attributes
          for (n = v.length - 1; n >= 0; n--)
            a[v[n].nodeName] = SVG.regex.isNumber.test(v[n].nodeValue) ? parseFloat(v[n].nodeValue) : v[n].nodeValue
          
          return a
          
        } else if (typeof a == 'object') {
          /* apply every attribute individually if an object is passed */
          for (v in a) this.attr(v, a[v])
          
        } else if (v === null) {
            /* remove value */
            this.node.removeAttribute(a)
          
        } else if (v == null) {
          /* act as a getter if the first and only argument is not an object */
          v = this.node.attributes[a]
          return v == null ? 
            SVG.defaults.attrs[a] :
          SVG.regex.isNumber.test(v.nodeValue) ?
            parseFloat(v.nodeValue) : v.nodeValue
        
        } else if (a == 'style') {
          /* redirect to the style method */
          return this.style(v)
        
        } else {
          /* BUG FIX: some browsers will render a stroke if a color is given even though stroke width is 0 */
          if (a == 'stroke-width')
            this.attr('stroke', parseFloat(v) > 0 ? this._stroke : null)
          else if (a == 'stroke')
            this._stroke = v
  
          /* convert image fill and stroke to patterns */
          if (a == 'fill' || a == 'stroke') {
            if (SVG.regex.isImage.test(v))
              v = this.doc().defs().image(v, 0, 0)
  
            if (v instanceof SVG.Image)
              v = this.doc().defs().pattern(0, 0, function() {
                this.add(v)
              })
          }
          
          /* ensure correct numeric values (also accepts NaN and Infinity) */
          if (typeof v === 'number')
            v = new SVG.Number(v)
  
          /* ensure full hex color */
          else if (SVG.Color.isColor(v))
            v = new SVG.Color(v)
          
          /* parse array values */
          else if (Array.isArray(v))
            v = new SVG.Array(v)
  
          /* if the passed attribute is leading... */
          if (a == 'leading') {
            /* ... call the leading method instead */
            if (this.leading)
              this.leading(v)
          } else {
            /* set given attribute on node */
            typeof n === 'string' ?
              this.node.setAttributeNS(n, a, v.toString()) :
              this.node.setAttribute(a, v.toString())
          }
          
          /* rebuild if required */
          if (this.rebuild && (a == 'font-size' || a == 'x'))
            this.rebuild(a, v)
        }
        
        return this
      }
      // Manage transformations
    , transform: function(o, v) {
        
        if (arguments.length == 0) {
          /* act as a getter if no argument is given */
          return this.trans
          
        } else if (typeof o === 'string') {
          /* act as a getter if only one string argument is given */
          if (arguments.length < 2)
            return this.trans[o]
          
          /* apply transformations as object if key value arguments are given*/
          var transform = {}
          transform[o] = v
          
          return this.transform(transform)
        }
        
        /* ... otherwise continue as a setter */
        var transform = []
        
        /* parse matrix */
        o = parseMatrix(o)
        
        /* merge values */
        for (v in o)
          if (o[v] != null)
            this.trans[v] = o[v]
        
        /* compile matrix */
        this.trans.matrix = this.trans.a
                    + ' ' + this.trans.b
                    + ' ' + this.trans.c
                    + ' ' + this.trans.d
                    + ' ' + this.trans.e
                    + ' ' + this.trans.f
        
        /* alias current transformations */
        o = this.trans
        
        /* add matrix */
        if (o.matrix != SVG.defaults.matrix)
          transform.push('matrix(' + o.matrix + ')')
        
        /* add rotation */
        if (o.rotation != 0)
          transform.push('rotate(' + o.rotation + ' ' + (o.cx == null ? this.bbox().cx : o.cx) + ' ' + (o.cy == null ? this.bbox().cy : o.cy) + ')')
        
        /* add scale */
        if (o.scaleX != 1 || o.scaleY != 1)
          transform.push('scale(' + o.scaleX + ' ' + o.scaleY + ')')
        
        /* add skew on x axis */
        if (o.skewX != 0)
          transform.push('skewX(' + o.skewX + ')')
        
        /* add skew on y axis */
        if (o.skewY != 0)
          transform.push('skewY(' + o.skewY + ')')
        
        /* add translation */
        if (o.x != 0 || o.y != 0)
          transform.push('translate(' + new SVG.Number(o.x / o.scaleX) + ' ' + new SVG.Number(o.y / o.scaleY) + ')')
        
        /* update transformations, even if there are none */
        if (transform.length == 0)
          this.node.removeAttribute('transform')
        else
          this.node.setAttribute('transform', transform.join(' '))
        
        return this
      }
      // Dynamic style generator
    , style: function(s, v) {
        if (arguments.length == 0) {
          /* get full style */
          return this.node.style.cssText || ''
        
        } else if (arguments.length < 2) {
          /* apply every style individually if an object is passed */
          if (typeof s == 'object') {
            for (v in s) this.style(v, s[v])
          
          } else if (SVG.regex.isCss.test(s)) {
            /* parse css string */
            s = s.split(';')
  
            /* apply every definition individually */
            for (var i = 0; i < s.length; i++) {
              v = s[i].split(':')
              this.style(v[0].replace(/\s+/g, ''), v[1])
            }
          } else {
            /* act as a getter if the first and only argument is not an object */
            return this.node.style[camelCase(s)]
          }
        
        } else {
          this.node.style[camelCase(s)] = v === null || SVG.regex.isBlank.test(v) ? '' : v
        }
        
        return this
      }
      // Get / set id
    , id: function(id) {
        return this.attr('id', id)
      }
      // Get bounding box
    , bbox: function() {
        return new SVG.BBox(this)
      }
      // Get rect box
    , rbox: function() {
        return new SVG.RBox(this)
      }
      // Checks whether the given point inside the bounding box of the element
    , inside: function(x, y) {
        var box = this.bbox()
        
        return x > box.x
            && y > box.y
            && x < box.x + box.width
            && y < box.y + box.height
      }
      // Show element
    , show: function() {
        return this.style('display', '')
      }
      // Hide element
    , hide: function() {
        return this.style('display', 'none')
      }
      // Is element visible?
    , visible: function() {
        return this.style('display') != 'none'
      }
      // Return id on string conversion
    , toString: function() {
        return this.attr('id')
      }
      // Return array of classes on the node
    , classes: function() {
        var classAttr = this.node.getAttribute('class')
        if (classAttr === null) {
          return []
        } else {
          return classAttr.trim().split(/\s+/)
        }
      }
      // Return true if class exists on the node, false otherwise
    , hasClass: function(className) {
        return this.classes().indexOf(className) != -1
      }
      // Add class to the node
    , addClass: function(className) {
        var classArray
        if (!(this.hasClass(className))) {
          classArray = this.classes()
          classArray.push(className)
          this.node.setAttribute('class', classArray.join(' '))
        }
        return this
      }
      // Remove class from the node
    , removeClass: function(className) {
        var classArray
        if (this.hasClass(className)) {
          classArray = this.classes().filter(function(c) {
            return c != className
          })
          this.node.setAttribute('class', classArray.join(' '))
        }
        return this
      }
      // Toggle the presence of a class on the node
    , toggleClass: function(className) {
        if (this.hasClass(className)) {
          this.removeClass(className)
        } else {
          this.addClass(className)
        }
        return this
      }
      // Get referenced element form attribute value
    , reference: function(attr) {
        return SVG.get(this.attr()[attr])
      }
      // Private: find svg parent by instance
    , _parent: function(parent) {
        var element = this
        
        while (element != null && !(element instanceof parent))
          element = element.parent
  
        return element
      }
    }
  })


  SVG.Parent = SVG.invent({
    // Initialize node
    create: function(element) {
      this.constructor.call(this, element)
    }
  
    // Inherit from
  , inherit: SVG.Element
  
    // Add class methods
  , extend: {
      // Returns all child elements
      children: function() {
        return this._children || (this._children = [])
      }
      // Add given element at a position
    , add: function(element, i) {
        if (!this.has(element)) {
          /* define insertion index if none given */
          i = i == null ? this.children().length : i
          
          /* remove references from previous parent */
          if (element.parent)
            element.parent.children().splice(element.parent.index(element), 1)
          
          /* add element references */
          this.children().splice(i, 0, element)
          this.node.insertBefore(element.node, this.node.childNodes[i] || null)
          element.parent = this
        }
  
        /* reposition defs */
        if (this._defs) {
          this.node.removeChild(this._defs.node)
          this.node.appendChild(this._defs.node)
        }
        
        return this
      }
      // Basically does the same as `add()` but returns the added element instead
    , put: function(element, i) {
        this.add(element, i)
        return element
      }
      // Checks if the given element is a child
    , has: function(element) {
        return this.index(element) >= 0
      }
      // Gets index of given element
    , index: function(element) {
        return this.children().indexOf(element)
      }
      // Get a element at the given index
    , get: function(i) {
        return this.children()[i]
      }
      // Get first child, skipping the defs node
    , first: function() {
        return this.children()[0]
      }
      // Get the last child
    , last: function() {
        return this.children()[this.children().length - 1]
      }
      // Iterates over all children and invokes a given block
    , each: function(block, deep) {
        var i, il
          , children = this.children()
        
        for (i = 0, il = children.length; i < il; i++) {
          if (children[i] instanceof SVG.Element)
            block.apply(children[i], [i, children])
  
          if (deep && (children[i] instanceof SVG.Container))
            children[i].each(block, deep)
        }
      
        return this
      }
      // Remove a child element at a position
    , removeElement: function(element) {
        this.children().splice(this.index(element), 1)
        this.node.removeChild(element.node)
        element.parent = null
        
        return this
      }
      // Remove all elements in this container
    , clear: function() {
        /* remove children */
        for (var i = this.children().length - 1; i >= 0; i--)
          this.removeElement(this.children()[i])
  
        /* remove defs node */
        if (this._defs)
          this._defs.clear()
  
        return this
      }
     , // Get defs
      defs: function() {
        return this.doc().defs()
      }
    }
    
  })


  SVG.Container = SVG.invent({
    // Initialize node
    create: function(element) {
      this.constructor.call(this, element)
    }
  
    // Inherit from
  , inherit: SVG.Parent
  
    // Add class methods
  , extend: {
      // Get the viewBox and calculate the zoom value
      viewbox: function(v) {
        if (arguments.length == 0)
          /* act as a getter if there are no arguments */
          return new SVG.ViewBox(this)
        
        /* otherwise act as a setter */
        v = arguments.length == 1 ?
          [v.x, v.y, v.width, v.height] :
          [].slice.call(arguments)
        
        return this.attr('viewBox', v)
      }
    }
    
  })

  SVG.FX = SVG.invent({
    // Initialize FX object
    create: function(element) {
      /* store target element */
      this.target = element
    }
  
    // Add class methods
  , extend: {
      // Add animation parameters and start animation
      animate: function(d, ease, delay) {
        var akeys, tkeys, skeys, key
          , element = this.target
          , fx = this
        
        /* dissect object if one is passed */
        if (typeof d == 'object') {
          delay = d.delay
          ease = d.ease
          d = d.duration
        }
  
        /* ensure default duration and easing */
        d = d == '=' ? d : d == null ? 1000 : new SVG.Number(d).valueOf()
        ease = ease || '<>'
  
        /* process values */
        fx.to = function(pos) {
          var i
  
          /* normalise pos */
          pos = pos < 0 ? 0 : pos > 1 ? 1 : pos
  
          /* collect attribute keys */
          if (akeys == null) {
            akeys = []
            for (key in fx.attrs)
              akeys.push(key)
  
            /* make sure morphable elements are scaled, translated and morphed all together */
            if (element.morphArray && (fx._plot || akeys.indexOf('points') > -1)) {
              /* get destination */
              var box
                , p = new element.morphArray(fx._plot || fx.attrs.points || element.array)
  
              /* add size */
              if (fx._size) p.size(fx._size.width.to, fx._size.height.to)
  
              /* add movement */
              box = p.bbox()
              if (fx._x) p.move(fx._x.to, box.y)
              else if (fx._cx) p.move(fx._cx.to - box.width / 2, box.y)
  
              box = p.bbox()
              if (fx._y) p.move(box.x, fx._y.to)
              else if (fx._cy) p.move(box.x, fx._cy.to - box.height / 2)
  
              /* delete element oriented changes */
              delete fx._x
              delete fx._y
              delete fx._cx
              delete fx._cy
              delete fx._size
  
              fx._plot = element.array.morph(p)
            }
          }
  
          /* collect transformation keys */
          if (tkeys == null) {
            tkeys = []
            for (key in fx.trans)
              tkeys.push(key)
          }
  
          /* collect style keys */
          if (skeys == null) {
            skeys = []
            for (key in fx.styles)
              skeys.push(key)
          }
  
          /* apply easing */
          pos = ease == '<>' ?
            (-Math.cos(pos * Math.PI) / 2) + 0.5 :
          ease == '>' ?
            Math.sin(pos * Math.PI / 2) :
          ease == '<' ?
            -Math.cos(pos * Math.PI / 2) + 1 :
          ease == '-' ?
            pos :
          typeof ease == 'function' ?
            ease(pos) :
            pos
          
          /* run plot function */
          if (fx._plot) {
            element.plot(fx._plot.at(pos))
  
          } else {
            /* run all x-position properties */
            if (fx._x)
              element.x(fx._x.at(pos))
            else if (fx._cx)
              element.cx(fx._cx.at(pos))
  
            /* run all y-position properties */
            if (fx._y)
              element.y(fx._y.at(pos))
            else if (fx._cy)
              element.cy(fx._cy.at(pos))
  
            /* run all size properties */
            if (fx._size)
              element.size(fx._size.width.at(pos), fx._size.height.at(pos))
          }
  
          /* run all viewbox properties */
          if (fx._viewbox)
            element.viewbox(
              fx._viewbox.x.at(pos)
            , fx._viewbox.y.at(pos)
            , fx._viewbox.width.at(pos)
            , fx._viewbox.height.at(pos)
            )
  
          /* run leading property */
          if (fx._leading)
            element.leading(fx._leading.at(pos))
  
          /* animate attributes */
          for (i = akeys.length - 1; i >= 0; i--)
            element.attr(akeys[i], at(fx.attrs[akeys[i]], pos))
  
          /* animate transformations */
          for (i = tkeys.length - 1; i >= 0; i--)
            element.transform(tkeys[i], at(fx.trans[tkeys[i]], pos))
  
          /* animate styles */
          for (i = skeys.length - 1; i >= 0; i--)
            element.style(skeys[i], at(fx.styles[skeys[i]], pos))
  
          /* callback for each keyframe */
          if (fx._during)
            fx._during.call(element, pos, function(from, to) {
              return at({ from: from, to: to }, pos)
            })
        }
        
        if (typeof d === 'number') {
          /* delay animation */
          this.timeout = setTimeout(function() {
            var start = new Date().getTime()
  
            /* initialize situation object */
            fx.situation = {
              interval: 1000 / 60
            , start:    start
            , play:     true
            , finish:   start + d
            , duration: d
            }
  
            /* render function */
            fx.render = function() {
              
              if (fx.situation.play === true) {
                // This code was borrowed from the emile.js micro framework by Thomas Fuchs, aka MadRobby.
                var time = new Date().getTime()
                  , pos = time > fx.situation.finish ? 1 : (time - fx.situation.start) / d
                
                /* process values */
                fx.to(pos)
                
                /* finish off animation */
                if (time > fx.situation.finish) {
                  if (fx._plot)
                    element.plot(new SVG.PointArray(fx._plot.destination).settle())
  
                  if (fx._loop === true || (typeof fx._loop == 'number' && fx._loop > 1)) {
                    if (typeof fx._loop == 'number')
                      --fx._loop
                    fx.animate(d, ease, delay)
                  } else {
                    fx._after ? fx._after.apply(element, [fx]) : fx.stop()
                  }
  
                } else {
                  fx.animationFrame = requestAnimationFrame(fx.render)
                }
              } else {
                fx.animationFrame = requestAnimationFrame(fx.render)
              }
              
            }
  
            /* start animation */
            fx.render()
            
          }, new SVG.Number(delay).valueOf())
        }
        
        return this
      }
      // Get bounding box of target element
    , bbox: function() {
        return this.target.bbox()
      }
      // Add animatable attributes
    , attr: function(a, v) {
        if (typeof a == 'object') {
          for (var key in a)
            this.attr(key, a[key])
        
        } else {
          var from = this.target.attr(a)
  
          this.attrs[a] = SVG.Color.isColor(from) ?
            new SVG.Color(from).morph(v) :
          SVG.regex.unit.test(from) ?
            new SVG.Number(from).morph(v) :
            { from: from, to: v }
        }
        
        return this
      }
      // Add animatable transformations
    , transform: function(o, v) {
        if (arguments.length == 1) {
          /* parse matrix string */
          o = parseMatrix(o)
          
          /* dlete matrixstring from object */
          delete o.matrix
          
          /* add rotation-center to transformations */
          this.target.trans.cx = o.cx || null
          this.target.trans.cy = o.cy || null
          
          delete o.cx
          delete o.cy
          
          /* store matrix values */
          for (v in o)
            this.trans[v] = { from: this.target.trans[v], to: o[v] }
          
        } else {
          /* apply transformations as object if key value arguments are given*/
          var transform = {}
          transform[o] = v
          
          this.transform(transform)
        }
        
        return this
      }
      // Add animatable styles
    , style: function(s, v) {
        if (typeof s == 'object')
          for (var key in s)
            this.style(key, s[key])
        
        else
          this.styles[s] = { from: this.target.style(s), to: v }
        
        return this
      }
      // Animatable x-axis
    , x: function(x) {
        this._x = new SVG.Number(this.target.x()).morph(x)
        
        return this
      }
      // Animatable y-axis
    , y: function(y) {
        this._y = new SVG.Number(this.target.y()).morph(y)
        
        return this
      }
      // Animatable center x-axis
    , cx: function(x) {
        this._cx = new SVG.Number(this.target.cx()).morph(x)
        
        return this
      }
      // Animatable center y-axis
    , cy: function(y) {
        this._cy = new SVG.Number(this.target.cy()).morph(y)
        
        return this
      }
      // Add animatable move
    , move: function(x, y) {
        return this.x(x).y(y)
      }
      // Add animatable center
    , center: function(x, y) {
        return this.cx(x).cy(y)
      }
      // Add animatable size
    , size: function(width, height) {
        if (this.target instanceof SVG.Text) {
          /* animate font size for Text elements */
          this.attr('font-size', width)
          
        } else {
          /* animate bbox based size for all other elements */
          var box = this.target.bbox()
  
          this._size = {
            width:  new SVG.Number(box.width).morph(width)
          , height: new SVG.Number(box.height).morph(height)
          }
        }
        
        return this
      }
      // Add animatable plot
    , plot: function(p) {
        this._plot = p
  
        return this
      }
      // Add leading method
    , leading: function(value) {
        if (this.target._leading)
          this._leading = new SVG.Number(this.target._leading).morph(value)
  
        return this
      }
      // Add animatable viewbox
    , viewbox: function(x, y, width, height) {
        if (this.target instanceof SVG.Container) {
          var box = this.target.viewbox()
          
          this._viewbox = {
            x:      new SVG.Number(box.x).morph(x)
          , y:      new SVG.Number(box.y).morph(y)
          , width:  new SVG.Number(box.width).morph(width)
          , height: new SVG.Number(box.height).morph(height)
          }
        }
        
        return this
      }
      // Add animateable gradient update
    , update: function(o) {
        if (this.target instanceof SVG.Stop) {
          if (o.opacity != null) this.attr('stop-opacity', o.opacity)
          if (o.color   != null) this.attr('stop-color', o.color)
          if (o.offset  != null) this.attr('offset', new SVG.Number(o.offset))
        }
  
        return this
      }
      // Add callback for each keyframe
    , during: function(during) {
        this._during = during
        
        return this
      }
      // Callback after animation
    , after: function(after) {
        this._after = after
        
        return this
      }
      // Make loopable
    , loop: function(times) {
        this._loop = times || true
  
        return this
      }
      // Stop running animation
    , stop: function(fulfill) {
        /* fulfill animation */
        if (fulfill === true) {
  
          this.animate(0)
  
          if (this._after)
            this._after.apply(this.target, [this])
  
        } else {
          /* stop current animation */
          clearTimeout(this.timeout)
          cancelAnimationFrame(this.animationFrame);
  
          /* reset storage for properties that need animation */
          this.attrs     = {}
          this.trans     = {}
          this.styles    = {}
          this.situation = {}
  
          /* delete destinations */
          delete this._x
          delete this._y
          delete this._cx
          delete this._cy
          delete this._size
          delete this._plot
          delete this._loop
          delete this._after
          delete this._during
          delete this._leading
          delete this._viewbox
        }
        
        return this
      }
      // Pause running animation
    , pause: function() {
        if (this.situation.play === true) {
          this.situation.play  = false
          this.situation.pause = new Date().getTime()
        }
  
        return this
      }
      // Play running animation
    , play: function() {
        if (this.situation.play === false) {
          var pause = new Date().getTime() - this.situation.pause
          
          this.situation.finish += pause
          this.situation.start  += pause
          this.situation.play    = true
        }
  
        return this
      }
      
    }
  
    // Define parent class
  , parent: SVG.Element
  
    // Add method to parent elements
  , construct: {
      // Get fx module or create a new one, then animate with given duration and ease
      animate: function(d, ease, delay) {
        return (this.fx || (this.fx = new SVG.FX(this))).stop().animate(d, ease, delay)
      }
      // Stop current animation; this is an alias to the fx instance
    , stop: function(fulfill) {
        if (this.fx)
          this.fx.stop(fulfill)
        
        return this
      }
      // Pause current animation
    , pause: function() {
        if (this.fx)
          this.fx.pause()
  
        return this
      }
      // Play paused current animation
    , play: function() {
        if (this.fx)
          this.fx.play()
  
        return this
      }
      
    }
  })


  SVG.extend(SVG.Element, SVG.FX, {
    // Relative move over x axis
    dx: function(x) {
      return this.x((this.target || this).x() + x)
    }
    // Relative move over y axis
  , dy: function(y) {
      return this.y((this.target || this).y() + y)
    }
    // Relative move over x and y axes
  , dmove: function(x, y) {
      return this.dx(x).dy(y)
    }
  
  })

  ;[  'click'
    , 'dblclick'
    , 'mousedown'
    , 'mouseup'
    , 'mouseover'
    , 'mouseout'
    , 'mousemove'
    // , 'mouseenter' -> not supported by IE
    // , 'mouseleave' -> not supported by IE
    , 'touchstart'
    , 'touchmove'
    , 'touchleave'
    , 'touchend'
    , 'touchcancel' ].forEach(function(event) {
    
    /* add event to SVG.Element */
    SVG.Element.prototype[event] = function(f) {
      var self = this
      
      /* bind event to element rather than element node */
      this.node['on' + event] = typeof f == 'function' ?
        function() { return f.apply(self, arguments) } : null
      
      return this
    }
    
  })
  
  // Initialize listeners stack
  SVG.listeners = []
  SVG.handlerMap = []
  
  // Only kept for consistency of API
  SVG.registerEvent = function(){};
  
  // Add event binder in the SVG namespace
  SVG.on = function(node, event, listener) {
    // create listener, get object-index
    var l     = listener.bind(node.instance || node)
      , index = (SVG.handlerMap.indexOf(node) + 1 || SVG.handlerMap.push(node)) - 1
      , ev    = event.split('.')[0]
      , ns    = event.split('.')[1] || '*'
      
    
    // ensure valid object
    SVG.listeners[index]         = SVG.listeners[index]         || {}
    SVG.listeners[index][ev]     = SVG.listeners[index][ev]     || {}
    SVG.listeners[index][ev][ns] = SVG.listeners[index][ev][ns] || {}
  
    // reference listener
    SVG.listeners[index][ev][ns][listener] = l
  
    // add listener
    node.addEventListener(ev, l, false)
  }
  
  // Add event unbinder in the SVG namespace
  SVG.off = function(node, event, listener) {
    var index = SVG.handlerMap.indexOf(node)
      , ev    = event && event.split('.')[0]
      , ns    = event && event.split('.')[1]
  
    if(index == -1) return
    
    if (listener) {
      // remove listener reference
      if (SVG.listeners[index][ev] && SVG.listeners[index][ev][ns || '*']) {
        // remove listener
        node.removeEventListener(ev, SVG.listeners[index][ev][ns || '*'][listener], false)
  
        delete SVG.listeners[index][ev][ns || '*'][listener]
      }
  
    } else if (ns) {
      // remove all listeners for the namespaced event
      if (SVG.listeners[index][ev] && SVG.listeners[index][ev][ns]) {
        for (listener in SVG.listeners[index][ev][ns])
          SVG.off(node, [ev, ns].join('.'), listener)
  
        delete SVG.listeners[index][ev][ns]
      }
  
    } else if (ev) {
      // remove all listeners for the event
      if (SVG.listeners[index][ev]) {
        for (namespace in SVG.listeners[index][ev])
          SVG.off(node, [ev, namespace].join('.'))
  
        delete SVG.listeners[index][ev]
      }
  
    } else {
      // remove all listeners on a given node
      for (event in SVG.listeners[index])
        SVG.off(node, event)
  
      delete SVG.listeners[index]
  
    }
  }
  
  //
  SVG.extend(SVG.Element, {
    // Bind given event to listener
    on: function(event, listener) {
      SVG.on(this.node, event, listener)
      
      return this
    }
    // Unbind event from listener
  , off: function(event, listener) {
      SVG.off(this.node, event, listener)
      
      return this
    }
    // Fire given event
  , fire: function(event, data) {
      
      // Dispatch event
      this.node.dispatchEvent(new CustomEvent(event, {detail:data}))
  
      return this
    }
  })

  SVG.Defs = SVG.invent({
    // Initialize node
    create: 'defs'
  
    // Inherit from
  , inherit: SVG.Container
    
  })

  SVG.G = SVG.invent({
    // Initialize node
    create: 'g'
  
    // Inherit from
  , inherit: SVG.Container
    
    // Add class methods
  , extend: {
      // Move over x-axis
      x: function(x) {
        return x == null ? this.trans.x : this.transform('x', x)
      }
      // Move over y-axis
    , y: function(y) {
        return y == null ? this.trans.y : this.transform('y', y)
      }
      // Move by center over x-axis
    , cx: function(x) {
        return x == null ? this.bbox().cx : this.x(x - this.bbox().width / 2)
      }
      // Move by center over y-axis
    , cy: function(y) {
        return y == null ? this.bbox().cy : this.y(y - this.bbox().height / 2)
      }
    }
    
    // Add parent method
  , construct: {
      // Create a group element
      group: function() {
        return this.put(new SVG.G)
      }
    }
  })

  SVG.extend(SVG.Element, {
    // Get all siblings, including myself
    siblings: function() {
      return this.parent.children()
    }
    // Get the curent position siblings
  , position: function() {
      return this.parent.index(this)
    }
    // Get the next element (will return null if there is none)
  , next: function() {
      return this.siblings()[this.position() + 1]
    }
    // Get the next element (will return null if there is none)
  , previous: function() {
      return this.siblings()[this.position() - 1]
    }
    // Send given element one step forward
  , forward: function() {
      var i = this.position()
      return this.parent.removeElement(this).put(this, i + 1)
    }
    // Send given element one step backward
  , backward: function() {
      var i = this.position()
      
      if (i > 0)
        this.parent.removeElement(this).add(this, i - 1)
  
      return this
    }
    // Send given element all the way to the front
  , front: function() {
      return this.parent.removeElement(this).put(this)
    }
    // Send given element all the way to the back
  , back: function() {
      if (this.position() > 0)
        this.parent.removeElement(this).add(this, 0)
      
      return this
    }
    // Inserts a given element before the targeted element
  , before: function(element) {
      element.remove()
  
      var i = this.position()
      
      this.parent.add(element, i)
  
      return this
    }
    // Insters a given element after the targeted element
  , after: function(element) {
      element.remove()
      
      var i = this.position()
      
      this.parent.add(element, i + 1)
  
      return this
    }
  
  })

  SVG.Mask = SVG.invent({
    // Initialize node
    create: function() {
      this.constructor.call(this, SVG.create('mask'))
  
      /* keep references to masked elements */
      this.targets = []
    }
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      // Unmask all masked elements and remove itself
      remove: function() {
        /* unmask all targets */
        for (var i = this.targets.length - 1; i >= 0; i--)
          if (this.targets[i])
            this.targets[i].unmask()
        delete this.targets
  
        /* remove mask from parent */
        this.parent.removeElement(this)
        
        return this
      }
    }
    
    // Add parent method
  , construct: {
      // Create masking element
      mask: function() {
        return this.defs().put(new SVG.Mask)
      }
    }
  })
  
  
  SVG.extend(SVG.Element, {
    // Distribute mask to svg element
    maskWith: function(element) {
      /* use given mask or create a new one */
      this.masker = element instanceof SVG.Mask ? element : this.parent.mask().add(element)
  
      /* store reverence on self in mask */
      this.masker.targets.push(this)
      
      /* apply mask */
      return this.attr('mask', 'url("#' + this.masker.attr('id') + '")')
    }
    // Unmask element
  , unmask: function() {
      delete this.masker
      return this.attr('mask', null)
    }
    
  })


  SVG.Clip = SVG.invent({
    // Initialize node
    create: function() {
      this.constructor.call(this, SVG.create('clipPath'))
  
      /* keep references to clipped elements */
      this.targets = []
    }
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      // Unclip all clipped elements and remove itself
      remove: function() {
        /* unclip all targets */
        for (var i = this.targets.length - 1; i >= 0; i--)
          if (this.targets[i])
            this.targets[i].unclip()
        delete this.targets
  
        /* remove clipPath from parent */
        this.parent.removeElement(this)
        
        return this
      }
    }
    
    // Add parent method
  , construct: {
      // Create clipping element
      clip: function() {
        return this.defs().put(new SVG.Clip)
      }
    }
  })
  
  //
  SVG.extend(SVG.Element, {
    // Distribute clipPath to svg element
    clipWith: function(element) {
      /* use given clip or create a new one */
      this.clipper = element instanceof SVG.Clip ? element : this.parent.clip().add(element)
  
      /* store reverence on self in mask */
      this.clipper.targets.push(this)
      
      /* apply mask */
      return this.attr('clip-path', 'url("#' + this.clipper.attr('id') + '")')
    }
    // Unclip element
  , unclip: function() {
      delete this.clipper
      return this.attr('clip-path', null)
    }
    
  })

  SVG.Gradient = SVG.invent({
    // Initialize node
    create: function(type) {
      this.constructor.call(this, SVG.create(type + 'Gradient'))
      
      /* store type */
      this.type = type
    }
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      // From position
      from: function(x, y) {
        return this.type == 'radial' ?
          this.attr({ fx: new SVG.Number(x), fy: new SVG.Number(y) }) :
          this.attr({ x1: new SVG.Number(x), y1: new SVG.Number(y) })
      }
      // To position
    , to: function(x, y) {
        return this.type == 'radial' ?
          this.attr({ cx: new SVG.Number(x), cy: new SVG.Number(y) }) :
          this.attr({ x2: new SVG.Number(x), y2: new SVG.Number(y) })
      }
      // Radius for radial gradient
    , radius: function(r) {
        return this.type == 'radial' ?
          this.attr({ r: new SVG.Number(r) }) :
          this
      }
      // Add a color stop
    , at: function(offset, color, opacity) {
        return this.put(new SVG.Stop).update(offset, color, opacity)
      }
      // Update gradient
    , update: function(block) {
        /* remove all stops */
        this.clear()
        
        /* invoke passed block */
        if (typeof block == 'function')
          block.call(this, this)
        
        return this
      }
      // Return the fill id
    , fill: function() {
        return 'url(#' + this.id() + ')'
      }
      // Alias string convertion to fill
    , toString: function() {
        return this.fill()
      }
    }
    
    // Add parent method
  , construct: {
      // Create gradient element in defs
      gradient: function(type, block) {
        return this.defs().gradient(type, block)
      }
    }
  })
  
  SVG.extend(SVG.Defs, {
    // define gradient
    gradient: function(type, block) {
      return this.put(new SVG.Gradient(type)).update(block)
    }
    
  })
  
  SVG.Stop = SVG.invent({
    // Initialize node
    create: 'stop'
  
    // Inherit from
  , inherit: SVG.Element
  
    // Add class methods
  , extend: {
      // add color stops
      update: function(o) {
        if (typeof o == 'number' || o instanceof SVG.Number) {
          o = {
            offset:  arguments[0]
          , color:   arguments[1]
          , opacity: arguments[2]
          }
        }
  
        /* set attributes */
        if (o.opacity != null) this.attr('stop-opacity', o.opacity)
        if (o.color   != null) this.attr('stop-color', o.color)
        if (o.offset  != null) this.attr('offset', new SVG.Number(o.offset))
  
        return this
      }
    }
  
  })


  SVG.Pattern = SVG.invent({
    // Initialize node
    create: 'pattern'
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      // Return the fill id
  	  fill: function() {
  	    return 'url(#' + this.id() + ')'
  	  }
  	  // Update pattern by rebuilding
  	, update: function(block) {
  			/* remove content */
        this.clear()
        
        /* invoke passed block */
        if (typeof block == 'function')
        	block.call(this, this)
        
        return this
  		}
  	  // Alias string convertion to fill
  	, toString: function() {
  	    return this.fill()
  	  }
    }
    
    // Add parent method
  , construct: {
      // Create pattern element in defs
  	  pattern: function(width, height, block) {
  	    return this.defs().pattern(width, height, block)
  	  }
    }
  })
  
  SVG.extend(SVG.Defs, {
    // Define gradient
    pattern: function(width, height, block) {
      return this.put(new SVG.Pattern).update(block).attr({
        x:            0
      , y:            0
      , width:        width
      , height:       height
      , patternUnits: 'userSpaceOnUse'
      })
    }
  
  })

  SVG.Doc = SVG.invent({
    // Initialize node
    create: function(element) {
      /* ensure the presence of a html element */
      this.parent = typeof element == 'string' ?
        document.getElementById(element) :
        element
      
      /* If the target is an svg element, use that element as the main wrapper.
         This allows svg.js to work with svg documents as well. */
      this.constructor
        .call(this, this.parent.nodeName == 'svg' ? this.parent : SVG.create('svg'))
      
      /* set svg element attributes */
      this
        .attr({ xmlns: SVG.ns, version: '1.1', width: '100%', height: '100%' })
        .attr('xmlns:xlink', SVG.xlink, SVG.xmlns)
      
      /* create the <defs> node */
      this._defs = new SVG.Defs
      this._defs.parent = this
      this.node.appendChild(this._defs.node)
  
      /* turn off sub pixel offset by default */
      this.doSpof = false
      
      /* ensure correct rendering */
      if (this.parent != this.node)
        this.stage()
    }
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      /* enable drawing */
      stage: function() {
        var element = this
  
        /* insert element */
        this.parent.appendChild(this.node)
  
        /* fix sub-pixel offset */
        element.spof()
        
        /* make sure sub-pixel offset is fixed every time the window is resized */
        SVG.on(window, 'resize', function() {
          element.spof()
        })
  
        return this
      }
  
      // Creates and returns defs element
    , defs: function() {
        return this._defs
      }
  
      // Fix for possible sub-pixel offset. See:
      // https://bugzilla.mozilla.org/show_bug.cgi?id=608812
    , spof: function() {
        if (this.doSpof) {
          var pos = this.node.getScreenCTM()
          
          if (pos)
            this
              .style('left', (-pos.e % 1) + 'px')
              .style('top',  (-pos.f % 1) + 'px')
        }
        
        return this
      }
  
      // Enable sub-pixel offset
    , fixSubPixelOffset: function() {
        this.doSpof = true
  
        return this
      }
      
        // Removes the doc from the DOM
    , remove: function() {
        if(this.parent) {
          this.parent.removeChild(this.node);
          this.parent = null;
        }
  
        return this;
      }
    }
    
  })


  SVG.Shape = SVG.invent({
    // Initialize node
    create: function(element) {
  	  this.constructor.call(this, element)
  	}
  
    // Inherit from
  , inherit: SVG.Element
  
  })

  SVG.Symbol = SVG.invent({
    // Initialize node
    create: 'symbol'
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add parent method
  , construct: {
      // Create a new symbol
      symbol: function() {
        return this.defs().put(new SVG.Symbol)
      }
    }
    
  })

  SVG.Use = SVG.invent({
    // Initialize node
    create: 'use'
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // Use element as a reference
      element: function(element, file) {
        /* store target element */
        this.target = element
  
        /* set lined element */
        return this.attr('href', (file || '') + '#' + element, SVG.xlink)
      }
    }
    
    // Add parent method
  , construct: {
      // Create a use element
      use: function(element, file) {
        return this.put(new SVG.Use).element(element, file)
      }
    }
  })

  SVG.Rect = SVG.invent({
  	// Initialize node
    create: 'rect'
  
  	// Inherit from
  , inherit: SVG.Shape
  	
  	// Add parent method
  , construct: {
    	// Create a rect element
    	rect: function(width, height) {
    	  return this.put(new SVG.Rect().size(width, height))
    	}
    	
  	}
  	
  })

  SVG.Ellipse = SVG.invent({
    // Initialize node
    create: 'ellipse'
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // Move over x-axis
      x: function(x) {
        return x == null ? this.cx() - this.attr('rx') : this.cx(x + this.attr('rx'))
      }
      // Move over y-axis
    , y: function(y) {
        return y == null ? this.cy() - this.attr('ry') : this.cy(y + this.attr('ry'))
      }
      // Move by center over x-axis
    , cx: function(x) {
        return x == null ? this.attr('cx') : this.attr('cx', new SVG.Number(x).divide(this.trans.scaleX))
      }
      // Move by center over y-axis
    , cy: function(y) {
        return y == null ? this.attr('cy') : this.attr('cy', new SVG.Number(y).divide(this.trans.scaleY))
      }
      // Set width of element
    , width: function(width) {
        return width == null ? this.attr('rx') * 2 : this.attr('rx', new SVG.Number(width).divide(2))
      }
      // Set height of element
    , height: function(height) {
        return height == null ? this.attr('ry') * 2 : this.attr('ry', new SVG.Number(height).divide(2))
      }
      // Custom size function
    , size: function(width, height) {
        var p = proportionalSize(this.bbox(), width, height)
  
        return this.attr({
          rx: new SVG.Number(p.width).divide(2)
        , ry: new SVG.Number(p.height).divide(2)
        })
      }
      
    }
  
    // Add parent method
  , construct: {
      // Create circle element, based on ellipse
      circle: function(size) {
        return this.ellipse(size, size)
      }
      // Create an ellipse
    , ellipse: function(width, height) {
        return this.put(new SVG.Ellipse).size(width, height).move(0, 0)
      }
      
    }
  
  })

  SVG.Line = SVG.invent({
    // Initialize node
    create: 'line'
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // Move over x-axis
      x: function(x) {
        var b = this.bbox()
        
        return x == null ? b.x : this.attr({
          x1: this.attr('x1') - b.x + x
        , x2: this.attr('x2') - b.x + x
        })
      }
      // Move over y-axis
    , y: function(y) {
        var b = this.bbox()
        
        return y == null ? b.y : this.attr({
          y1: this.attr('y1') - b.y + y
        , y2: this.attr('y2') - b.y + y
        })
      }
      // Move by center over x-axis
    , cx: function(x) {
        var half = this.bbox().width / 2
        return x == null ? this.x() + half : this.x(x - half)
      }
      // Move by center over y-axis
    , cy: function(y) {
        var half = this.bbox().height / 2
        return y == null ? this.y() + half : this.y(y - half)
      }
      // Set width of element
    , width: function(width) {
        var b = this.bbox()
  
        return width == null ? b.width : this.attr(this.attr('x1') < this.attr('x2') ? 'x2' : 'x1', b.x + width)
      }
      // Set height of element
    , height: function(height) {
        var b = this.bbox()
  
        return height == null ? b.height : this.attr(this.attr('y1') < this.attr('y2') ? 'y2' : 'y1', b.y + height)
      }
      // Set line size by width and height
    , size: function(width, height) {
        var p = proportionalSize(this.bbox(), width, height)
  
        return this.width(p.width).height(p.height)
      }
      // Set path data
    , plot: function(x1, y1, x2, y2) {
        return this.attr({
          x1: x1
        , y1: y1
        , x2: x2
        , y2: y2
        })
      }
    }
    
    // Add parent method
  , construct: {
      // Create a line element
      line: function(x1, y1, x2, y2) {
        return this.put(new SVG.Line().plot(x1, y1, x2, y2))
      }
    }
  })


  SVG.Polyline = SVG.invent({
    // Initialize node
    create: 'polyline'
  
    // Inherit from
  , inherit: SVG.Shape
    
    // Add parent method
  , construct: {
      // Create a wrapped polyline element
      polyline: function(p) {
        return this.put(new SVG.Polyline).plot(p)
      }
    }
  })
  
  SVG.Polygon = SVG.invent({
    // Initialize node
    create: 'polygon'
  
    // Inherit from
  , inherit: SVG.Shape
    
    // Add parent method
  , construct: {
      // Create a wrapped polygon element
      polygon: function(p) {
        return this.put(new SVG.Polygon).plot(p)
      }
    }
  })
  
  // Add polygon-specific functions
  SVG.extend(SVG.Polyline, SVG.Polygon, {
    // Define morphable array
    morphArray:  SVG.PointArray
    // Plot new path
  , plot: function(p) {
      return this.attr('points', (this.array = new SVG.PointArray(p, [[0,0]])))
    }
    // Move by left top corner
  , move: function(x, y) {
      return this.attr('points', this.array.move(x, y))
    }
    // Move by left top corner over x-axis
  , x: function(x) {
      return x == null ? this.bbox().x : this.move(x, this.bbox().y)
    }
    // Move by left top corner over y-axis
  , y: function(y) {
      return y == null ? this.bbox().y : this.move(this.bbox().x, y)
    }
    // Set width of element
  , width: function(width) {
      var b = this.bbox()
  
      return width == null ? b.width : this.size(width, b.height)
    }
    // Set height of element
  , height: function(height) {
      var b = this.bbox()
  
      return height == null ? b.height : this.size(b.width, height) 
    }
    // Set element size to given width and height
  , size: function(width, height) {
      var p = proportionalSize(this.bbox(), width, height)
  
      return this.attr('points', this.array.size(p.width, p.height))
    }
  
  })

  SVG.Path = SVG.invent({
    // Initialize node
    create: 'path'
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // Plot new poly points
      plot: function(p) {
        return this.attr('d', (this.array = new SVG.PathArray(p, [['M', 0, 0]])))
      }
      // Move by left top corner
    , move: function(x, y) {
        return this.attr('d', this.array.move(x, y))
      }
      // Move by left top corner over x-axis
    , x: function(x) {
        return x == null ? this.bbox().x : this.move(x, this.bbox().y)
      }
      // Move by left top corner over y-axis
    , y: function(y) {
        return y == null ? this.bbox().y : this.move(this.bbox().x, y)
      }
      // Set element size to given width and height
    , size: function(width, height) {
        var p = proportionalSize(this.bbox(), width, height)
        
        return this.attr('d', this.array.size(p.width, p.height))
      }
      // Set width of element
    , width: function(width) {
        return width == null ? this.bbox().width : this.size(width, this.bbox().height)
      }
      // Set height of element
    , height: function(height) {
        return height == null ? this.bbox().height : this.size(this.bbox().width, height)
      }
      
    }
    
    // Add parent method
  , construct: {
      // Create a wrapped path element
      path: function(d) {
        return this.put(new SVG.Path).plot(d)
      }
    }
  })

  SVG.Image = SVG.invent({
    // Initialize node
    create: 'image'
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // (re)load image
      load: function(url) {
        if (!url) return this
  
        var self = this
          , img  = document.createElement('img')
        
        /* preload image */
        img.onload = function() {
          var p = self.doc(SVG.Pattern)
  
          /* ensure image size */
          if (self.width() == 0 && self.height() == 0)
            self.size(img.width, img.height)
  
          /* ensure pattern size if not set */
          if (p && p.width() == 0 && p.height() == 0)
            p.size(self.width(), self.height())
          
          /* callback */
          if (typeof self._loaded === 'function')
            self._loaded.call(self, {
              width:  img.width
            , height: img.height
            , ratio:  img.width / img.height
            , url:    url
            })
        }
  
        return this.attr('href', (img.src = this.src = url), SVG.xlink)
      }
      // Add loade callback
    , loaded: function(loaded) {
        this._loaded = loaded
        return this
      }
    }
    
    // Add parent method
  , construct: {
      // Create image element, load image and set its size
      image: function(source, width, height) {
        return this.put(new SVG.Image).load(source).size(width || 0, height || width || 0)
      }
    }
  
  })

  SVG.Text = SVG.invent({
    // Initialize node
    create: function() {
      this.constructor.call(this, SVG.create('text'))
      
      this._leading = new SVG.Number(1.3)    /* store leading value for rebuilding */
      this._rebuild = true                   /* enable automatic updating of dy values */
      this._build   = false                  /* disable build mode for adding multiple lines */
  
      /* set default font */
      this.attr('font-family', SVG.defaults.attrs['font-family'])
    }
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // Move over x-axis
      x: function(x) {
        /* act as getter */
        if (x == null)
          return this.attr('x')
        
        /* move lines as well if no textPath is present */
        if (!this.textPath)
          this.lines.each(function() { if (this.newLined) this.x(x) })
  
        return this.attr('x', x)
      }
      // Move over y-axis
    , y: function(y) {
        var oy = this.attr('y')
          , o  = typeof oy === 'number' ? oy - this.bbox().y : 0
  
        /* act as getter */
        if (y == null)
          return typeof oy === 'number' ? oy - o : oy
  
        return this.attr('y', typeof y === 'number' ? y + o : y)
      }
      // Move center over x-axis
    , cx: function(x) {
        return x == null ? this.bbox().cx : this.x(x - this.bbox().width / 2)
      }
      // Move center over y-axis
    , cy: function(y) {
        return y == null ? this.bbox().cy : this.y(y - this.bbox().height / 2)
      }
      // Set the text content
    , text: function(text) {
        /* act as getter */
        if (typeof text === 'undefined') return this.content
        
        /* remove existing content */
        this.clear().build(true)
        
        if (typeof text === 'function') {
          /* call block */
          text.call(this, this)
  
        } else {
          /* store text and make sure text is not blank */
          text = (this.content = text).split('\n')
          
          /* build new lines */
          for (var i = 0, il = text.length; i < il; i++)
            this.tspan(text[i]).newLine()
        }
        
        /* disable build mode and rebuild lines */
        return this.build(false).rebuild()
      }
      // Set font size
    , size: function(size) {
        return this.attr('font-size', size).rebuild()
      }
      // Set / get leading
    , leading: function(value) {
        /* act as getter */
        if (value == null)
          return this._leading
        
        /* act as setter */
        this._leading = new SVG.Number(value)
        
        return this.rebuild()
      }
      // Rebuild appearance type
    , rebuild: function(rebuild) {
        /* store new rebuild flag if given */
        if (typeof rebuild == 'boolean')
          this._rebuild = rebuild
  
        /* define position of all lines */
        if (this._rebuild) {
          var self = this
          
          this.lines.each(function() {
            if (this.newLined) {
              if (!this.textPath)
                this.attr('x', self.attr('x'))
              this.attr('dy', self._leading * new SVG.Number(self.attr('font-size'))) 
            }
          })
  
          this.fire('rebuild')
        }
  
        return this
      }
      // Enable / disable build mode
    , build: function(build) {
        this._build = !!build
        return this
      }
    }
    
    // Add parent method
  , construct: {
      // Create text element
      text: function(text) {
        return this.put(new SVG.Text).text(text)
      }
      // Create plain text element
    , plain: function(text) {
        return this.put(new SVG.Text).plain(text)
      }
    }
  
  })
  
  SVG.TSpan = SVG.invent({
    // Initialize node
    create: 'tspan'
  
    // Inherit from
  , inherit: SVG.Shape
  
    // Add class methods
  , extend: {
      // Set text content
      text: function(text) {
        typeof text === 'function' ? text.call(this, this) : this.plain(text)
  
        return this
      }
      // Shortcut dx
    , dx: function(dx) {
        return this.attr('dx', dx)
      }
      // Shortcut dy
    , dy: function(dy) {
        return this.attr('dy', dy)
      }
      // Create new line
    , newLine: function() {
        /* fetch text parent */
        var t = this.doc(SVG.Text)
  
        /* mark new line */
        this.newLined = true
  
        /* apply new hy¡n */
        return this.dy(t._leading * t.attr('font-size')).attr('x', t.x())
      }
    }
    
  })
  
  SVG.extend(SVG.Text, SVG.TSpan, {
    // Create plain text node
    plain: function(text) {
      /* clear if build mode is disabled */
      if (this._build === false)
        this.clear()
  
      /* create text node */
      this.node.appendChild(document.createTextNode((this.content = text)))
      
      return this
    }
    // Create a tspan
  , tspan: function(text) {
      var node  = (this.textPath || this).node
        , tspan = new SVG.TSpan
  
      /* clear if build mode is disabled */
      if (this._build === false)
        this.clear()
      
      /* add new tspan and reference */
      node.appendChild(tspan.node)
      tspan.parent = this
  
      /* only first level tspans are considered to be "lines" */
      if (this instanceof SVG.Text)
        this.lines.add(tspan)
  
      return tspan.text(text)
    }
    // Clear all lines
  , clear: function() {
      var node = (this.textPath || this).node
  
      /* remove existing child nodes */
      while (node.hasChildNodes())
        node.removeChild(node.lastChild)
      
      /* reset content references  */
      if (this instanceof SVG.Text) {
        delete this.lines
        this.lines = new SVG.Set
        this.content = ''
      }
      
      return this
    }
    // Get length of text element
  , length: function() {
      return this.node.getComputedTextLength()
    }
  })


  SVG.TextPath = SVG.invent({
    // Initialize node
    create: 'textPath'
  
    // Inherit from
  , inherit: SVG.Element
  
    // Define parent class
  , parent: SVG.Text
  
    // Add parent method
  , construct: {
      // Create path for text to run on
      path: function(d) {
        /* create textPath element */
        this.textPath = new SVG.TextPath
  
        /* move lines to textpath */
        while(this.node.hasChildNodes())
          this.textPath.node.appendChild(this.node.firstChild)
  
        /* add textPath element as child node */
        this.node.appendChild(this.textPath.node)
  
        /* create path in defs */
        this.track = this.doc().defs().path(d)
  
        /* create circular reference */
        this.textPath.parent = this
  
        /* link textPath to path and add content */
        this.textPath.attr('href', '#' + this.track, SVG.xlink)
  
        return this
      }
      // Plot path if any
    , plot: function(d) {
        if (this.track) this.track.plot(d)
        return this
      }
    }
  })

  SVG.Nested = SVG.invent({
    // Initialize node
    create: function() {
      this.constructor.call(this, SVG.create('svg'))
      
      this.style('overflow', 'visible')
    }
  
    // Inherit from
  , inherit: SVG.Container
    
    // Add parent method
  , construct: {
      // Create nested svg document
      nested: function() {
        return this.put(new SVG.Nested)
      }
    }
  })

  SVG.A = SVG.invent({
    // Initialize node
    create: 'a'
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      // Link url
      to: function(url) {
        return this.attr('href', url, SVG.xlink)
      }
      // Link show attribute
    , show: function(target) {
        return this.attr('show', target, SVG.xlink)
      }
      // Link target attribute
    , target: function(target) {
        return this.attr('target', target)
      }
    }
    
    // Add parent method
  , construct: {
      // Create a hyperlink element
      link: function(url) {
        return this.put(new SVG.A).to(url)
      }
    }
  })
  
  SVG.extend(SVG.Element, {
    // Create a hyperlink element
    linkTo: function(url) {
      var link = new SVG.A
  
      if (typeof url == 'function')
        url.call(link, link)
      else
        link.to(url)
  
      return this.parent.put(link).put(this)
    }
    
  })

  SVG.Marker = SVG.invent({
    // Initialize node
    create: 'marker'
  
    // Inherit from
  , inherit: SVG.Container
  
    // Add class methods
  , extend: {
      // Set width of element
      width: function(width) {
        return this.attr('markerWidth', width)
      }
      // Set height of element
    , height: function(height) {
        return this.attr('markerHeight', height)
      }
      // Set marker refX and refY
    , ref: function(x, y) {
        return this.attr('refX', x).attr('refY', y)
      }
      // Update marker
    , update: function(block) {
        /* remove all content */
        this.clear()
        
        /* invoke passed block */
        if (typeof block == 'function')
          block.call(this, this)
        
        return this
      }
      // Return the fill id
    , toString: function() {
        return 'url(#' + this.id() + ')'
      }
    }
  
    // Add parent method
  , construct: {
      marker: function(width, height, block) {
        // Create marker element in defs
        return this.defs().marker(width, height, block)
      }
    }
  
  })
  
  SVG.extend(SVG.Defs, {
    // Create marker
    marker: function(width, height, block) {
      // Set default viewbox to match the width and height, set ref to cx and cy and set orient to auto
      return this.put(new SVG.Marker)
        .size(width, height)
        .ref(width / 2, height / 2)
        .viewbox(0, 0, width, height)
        .attr('orient', 'auto')
        .update(block)
    }
    
  })
  
  SVG.extend(SVG.Line, SVG.Polyline, SVG.Polygon, SVG.Path, {
    // Create and attach markers
    marker: function(marker, width, height, block) {
      var attr = ['marker']
  
      // Build attribute name
      if (marker != 'all') attr.push(marker)
      attr = attr.join('-')
  
      // Set marker attribute
      marker = arguments[1] instanceof SVG.Marker ?
        arguments[1] :
        this.doc().marker(width, height, block)
      
      return this.attr(attr, marker)
    }
    
  })

  var sugar = {
    stroke: ['color', 'width', 'opacity', 'linecap', 'linejoin', 'miterlimit', 'dasharray', 'dashoffset']
  , fill:   ['color', 'opacity', 'rule']
  , prefix: function(t, a) {
      return a == 'color' ? t : t + '-' + a
    }
  }
  
  /* Add sugar for fill and stroke */
  ;['fill', 'stroke'].forEach(function(m) {
    var i, extension = {}
    
    extension[m] = function(o) {
      if (typeof o == 'string' || SVG.Color.isRgb(o) || (o && typeof o.fill === 'function'))
        this.attr(m, o)
  
      else
        /* set all attributes from sugar.fill and sugar.stroke list */
        for (i = sugar[m].length - 1; i >= 0; i--)
          if (o[sugar[m][i]] != null)
            this.attr(sugar.prefix(m, sugar[m][i]), o[sugar[m][i]])
      
      return this
    }
    
    SVG.extend(SVG.Element, SVG.FX, extension)
    
  })
  
  SVG.extend(SVG.Element, SVG.FX, {
    // Rotation
    rotate: function(deg, x, y) {
      return this.transform({
        rotation: deg || 0
      , cx: x
      , cy: y
      })
    }
    // Skew
  , skew: function(x, y) {
      return this.transform({
        skewX: x || 0
      , skewY: y || 0
      })
    }
    // Scale
  , scale: function(x, y) {
      return this.transform({
        scaleX: x
      , scaleY: y == null ? x : y
      })
    }
    // Translate
  , translate: function(x, y) {
      return this.transform({
        x: x
      , y: y
      })
    }
    // Matrix
  , matrix: function(m) {
      return this.transform({ matrix: m })
    }
    // Opacity
  , opacity: function(value) {
      return this.attr('opacity', value)
    }
  
  })
  
  SVG.extend(SVG.Rect, SVG.Ellipse, SVG.FX, {
    // Add x and y radius
    radius: function(x, y) {
      return this.attr({ rx: x, ry: y || x })
    }
  
  })
  
  SVG.extend(SVG.Path, {
    // Get path length
    length: function() {
      return this.node.getTotalLength()
    }
    // Get point at length
  , pointAt: function(length) {
      return this.node.getPointAtLength(length)
    }
  
  })
  
  SVG.extend(SVG.Parent, SVG.Text, SVG.FX, {
    // Set font 
    font: function(o) {
      for (var k in o)
        k == 'leading' ?
          this.leading(o[k]) :
        k == 'anchor' ?
          this.attr('text-anchor', o[k]) :
        k == 'size' || k == 'family' || k == 'weight' || k == 'stretch' || k == 'variant' || k == 'style' ?
          this.attr('font-'+ k, o[k]) :
          this.attr(k, o[k])
      
      return this
    }
    
  })
  


  SVG.Set = SVG.invent({
    // Initialize
    create: function() {
      /* set initial state */
      this.clear()
    }
  
    // Add class methods
  , extend: {
      // Add element to set
      add: function() {
        var i, il, elements = [].slice.call(arguments)
  
        for (i = 0, il = elements.length; i < il; i++)
          this.members.push(elements[i])
        
        return this
      }
      // Remove element from set
    , remove: function(element) {
        var i = this.index(element)
        
        /* remove given child */
        if (i > -1)
          this.members.splice(i, 1)
  
        return this
      }
      // Iterate over all members
    , each: function(block) {
        for (var i = 0, il = this.members.length; i < il; i++)
          block.apply(this.members[i], [i, this.members])
  
        return this
      }
      // Restore to defaults
    , clear: function() {
        /* initialize store */
        this.members = []
  
        return this
      }
      // Checks if a given element is present in set
    , has: function(element) {
        return this.index(element) >= 0
      }
      // retuns index of given element in set
    , index: function(element) {
        return this.members.indexOf(element)
      }
      // Get member at given index
    , get: function(i) {
        return this.members[i]
      }
      // Get first member
    , first: function() {
        return this.get(0)
      }
      // Get last member
    , last: function() {
        return this.get(this.members.length - 1)
      }
      // Default value
    , valueOf: function() {
        return this.members
      }
      // Get the bounding box of all members included or empty box if set has no items
    , bbox: function(){
        var box = new SVG.BBox()
  
        /* return an empty box of there are no members */
        if (this.members.length == 0)
          return box
  
        /* get the first rbox and update the target bbox */
        var rbox = this.members[0].rbox()
        box.x      = rbox.x
        box.y      = rbox.y
        box.width  = rbox.width
        box.height = rbox.height
  
        this.each(function() {
          /* user rbox for correct position and visual representation */
          box = box.merge(this.rbox())
        })
  
        return box
      }
    }
    
    // Add parent method
  , construct: {
      // Create a new set
      set: function() {
        return new SVG.Set
      }
    }
  })
  
  SVG.SetFX = SVG.invent({
    // Initialize node
    create: function(set) {
      /* store reference to set */
      this.set = set
    }
  
  })
  
  // Alias methods
  SVG.Set.inherit = function() {
    var m
      , methods = []
    
    /* gather shape methods */
    for(var m in SVG.Shape.prototype)
      if (typeof SVG.Shape.prototype[m] == 'function' && typeof SVG.Set.prototype[m] != 'function')
        methods.push(m)
  
    /* apply shape aliasses */
    methods.forEach(function(method) {
      SVG.Set.prototype[method] = function() {
        for (var i = 0, il = this.members.length; i < il; i++)
          if (this.members[i] && typeof this.members[i][method] == 'function')
            this.members[i][method].apply(this.members[i], arguments)
  
        return method == 'animate' ? (this.fx || (this.fx = new SVG.SetFX(this))) : this
      }
    })
  
    /* clear methods for the next round */
    methods = []
  
    /* gather fx methods */
    for(var m in SVG.FX.prototype)
      if (typeof SVG.FX.prototype[m] == 'function' && typeof SVG.SetFX.prototype[m] != 'function')
        methods.push(m)
  
    /* apply fx aliasses */
    methods.forEach(function(method) {
      SVG.SetFX.prototype[method] = function() {
        for (var i = 0, il = this.set.members.length; i < il; i++)
          this.set.members[i].fx[method].apply(this.set.members[i].fx, arguments)
  
        return this
      }
    })
  }
  
  


  SVG.extend(SVG.Element, {
  	// Store data values on svg nodes
    data: function(a, v, r) {
    	if (typeof a == 'object') {
    		for (v in a)
    			this.data(v, a[v])
  
      } else if (arguments.length < 2) {
        try {
          return JSON.parse(this.attr('data-' + a))
        } catch(e) {
          return this.attr('data-' + a)
        }
        
      } else {
        this.attr(
          'data-' + a
        , v === null ?
            null :
          r === true || typeof v === 'string' || typeof v === 'number' ?
            v :
            JSON.stringify(v)
        )
      }
      
      return this
    }
  })

  SVG.extend(SVG.Element, {
    // Remember arbitrary data
    remember: function(k, v) {
      /* remember every item in an object individually */
      if (typeof arguments[0] == 'object')
        for (var v in k)
          this.remember(v, k[v])
  
      /* retrieve memory */
      else if (arguments.length == 1)
        return this.memory()[k]
  
      /* store memory */
      else
        this.memory()[k] = v
  
      return this
    }
  
    // Erase a given memory
  , forget: function() {
      if (arguments.length == 0)
        this._memory = {}
      else
        for (var i = arguments.length - 1; i >= 0; i--)
          delete this.memory()[arguments[i]]
  
      return this
    }
  
    // Initialize or return local memory object
  , memory: function() {
      return this._memory || (this._memory = {})
    }
  
  })

  function camelCase(s) { 
    return s.toLowerCase().replace(/-(.)/g, function(m, g) {
      return g.toUpperCase()
    })
  }
  
  // Ensure to six-based hex 
  function fullHex(hex) {
    return hex.length == 4 ?
      [ '#',
        hex.substring(1, 2), hex.substring(1, 2)
      , hex.substring(2, 3), hex.substring(2, 3)
      , hex.substring(3, 4), hex.substring(3, 4)
      ].join('') : hex
  }
  
  // Component to hex value
  function compToHex(comp) {
    var hex = comp.toString(16)
    return hex.length == 1 ? '0' + hex : hex
  }
  
  // Calculate proportional width and height values when necessary
  function proportionalSize(box, width, height) {
    if (width == null || height == null) {
      if (height == null)
        height = box.height / box.width * width
      else if (width == null)
        width = box.width / box.height * height
    }
    
    return {
      width:  width
    , height: height
    }
  }
  
  // Calculate position according to from and to
  function at(o, pos) {
    /* number recalculation (don't bother converting to SVG.Number for performance reasons) */
    return typeof o.from == 'number' ?
      o.from + (o.to - o.from) * pos :
    
    /* instance recalculation */
    o instanceof SVG.Color || o instanceof SVG.Number ? o.at(pos) :
    
    /* for all other values wait until pos has reached 1 to return the final value */
    pos < 1 ? o.from : o.to
  }
  
  // PathArray Helpers
  function arrayToString(a) {
    for (var i = 0, il = a.length, s = ''; i < il; i++) {
      s += a[i][0]
  
      if (a[i][1] != null) {
        s += a[i][1]
  
        if (a[i][2] != null) {
          s += ' '
          s += a[i][2]
  
          if (a[i][3] != null) {
            s += ' '
            s += a[i][3]
            s += ' '
            s += a[i][4]
  
            if (a[i][5] != null) {
              s += ' '
              s += a[i][5]
              s += ' '
              s += a[i][6]
  
              if (a[i][7] != null) {
                s += ' '
                s += a[i][7]
              }
            }
          }
        }
      }
    }
    
    return s + ' '
  }
  
  // Add more bounding box properties
  function boxProperties(b) {
    b.x2 = b.x + b.width
    b.y2 = b.y + b.height
    b.cx = b.x + b.width / 2
    b.cy = b.y + b.height / 2
  }
  
  // Parse a matrix string
  function parseMatrix(o) {
    if (o.matrix) {
      /* split matrix string */
      var m = o.matrix.replace(/\s/g, '').split(',')
      
      /* pasrse values */
      if (m.length == 6) {
        o.a = parseFloat(m[0])
        o.b = parseFloat(m[1])
        o.c = parseFloat(m[2])
        o.d = parseFloat(m[3])
        o.e = parseFloat(m[4])
        o.f = parseFloat(m[5])
      }
    }
    
    return o
  }
  
  // Get id from reference string
  function idFromReference(url) {
    var m = url.toString().match(SVG.regex.reference)
  
    if (m) return m[1]
  }


  return SVG
}));

///#source 1 1 /Scripts/svg/svg.math.js
// svg.path.js 0.2 - Copyright (c) 2013 Nils Lagerkvist - Licensed under the MIT license

(function(){


	SVG.math = {
		angle: function(p1, p2, p3){
			if (p3){
				return Math.abs(SVG.math.angle(p1, p3) - SVG.math.angle(p2, p3));
			}

			var angle = Math.atan2(p2.y - p1.y, p2.x - p1.x);

			while (angle < 0){
				angle += 2 * Math.PI;
			}

			return angle;
		},

		rad: function(degree){
			return degree * Math.PI / 180;
		},

		deg: function(radians){
			return radians * 180 / Math.PI;
		},

		snapToAngle: function(angle, directions){
			var minDiff = 100,
				diff,
				i,
				length;

			// Find the smallest value in the array and add 2*PI to it
			directions.push(Math.min.apply( Math, directions ) + 2 * Math.PI);

			length = directions.length;

			while (angle > 2 * Math.PI){
				angle -= 2 * Math.PI;
			}

			while (angle < 0){
				angle += 2 * Math.PI;
			}

			for (i = 0; i < length; i += 1){
				diff = Math.abs(angle - directions[i]);
				if (diff > minDiff){
					return directions[i-1];
				}
				minDiff = diff;
			}

			// This is a special case when we match on the smallest angle + 2*PI
			return directions[0];
		},

		// linear interpolation of two values
		lerp: function(a, b, x) {
			return a + x * (b - a);
		}

	};
	
	

	SVG.math.Point = function Point(x, y){
		this.x = x;
		this.y = y;
	};

	SVG.math.Point.attr = {
		stroke: '#000',
		fill: 'none',
		radius: 5
	};

	SVG.extend(SVG.math.Point, {
		/** draw(svg, attr)
         * svg - the SVG to add (draw) the point on
         * attr - custom attributes for the circle
         *
         * To remove the point form the SVG set the svg to null by calling 
         * draw function without any arguments
         */
		draw: function(svg, attr){
			if (svg){
				attr = attr || SVG.math.Point.attr;
				this.svg = svg;
				this.circle = svg.circle(attr.radius).attr('cx',this.x).attr('cy',this.y).attr(attr);
			}
			else if (this.circle){
				this.circle.remove();
				delete this.svg;
				delete this.circle;
			}

			return this;
		}
	});

	SVG.math.Line = function Line(p1, p2){
		this.update(p1, p2);
	};

	SVG.math.Line.attr = {
		stroke: '#000',
		fill: 'none'
	};

	SVG.extend(SVG.math.Line, {
		update: function(p1, p2){               
			this.p1 = p1;
			this.p2 = p2;		
		
			this.a = this.p2.y - this.p1.y;
			this.b = this.p1.x - this.p2.x;

			this.c = p1.x * p2.y - p2.x * p1.y; 

			return this;
		},
		draw: function(svg, options){
			if (svg){
				attr = attr || SVG.math.Line.attr;
				this.svg = svg;
				this.line = svg.line(p1.x, p1.y, p2.x, p2.y).attr(options);
			}
			else if (this.line){
				this.line.remove();
				delete this.svg;
				delete this.line;
			}

			return this
		},
		parallel: function(line){
			return (this.a * line.b - line.a * this.b) === 0;			
		},

		move: function(from, towards, distance){
			var sign = (from.x > towards.x) ? -1 :
				from.x < towards.x ? 1 : 
				from.y > towards.y ? -1 : 1; // The special case when from.x == towards.x

			var theta = Math.atan(Math.abs(this.p1.y - this.p2.y) / Math.abs(this.p1.x - this.p2.x));
			var dy = distance * Math.sin(theta);
			var dx = distance * Math.cos(theta);
			return new SVG.math.Point(
				from.x + sign * dx, 
				from.y + sign * dy
			);
		},

		intersection: function(line){
			var det = this.a * line.b - line.a * this.b;

			return {
				parallel: (det === 0),
				x: (line.b * this.c - this.b * line.c) / det,
				y: (this.a * line.c - line.a * this.c) / det
			};

		},
		
		midPoint: function(){
			return this.interpolatedPoint(0.5);
		},

		segmentLengthSquared: function() {
			var dx = this.p2.x - this.p1.x;
			var dy = this.p2.y - this.p1.y;
			return dx * dx + dy * dy;
		},

		closestLinearInterpolation: function(p) {
			var dx = this.p2.x - this.p1.x;
			var dy = this.p2.y - this.p1.y;

			return ((p.x - this.p1.x) * dx + (p.y - this.p1.y) * dy) /
				this.segmentLengthSquared();
		},

		interpolatedPoint: function(t) {
			return {
				x: SVG.math.lerp(this.p1.x, this.p2.x, t),
				y: SVG.math.lerp(this.p1.y, this.p2.y, t)
			};
		},

		closestPoint: function(p) {
			return this.interpolatedPoint(
				this.closestLinearInterpolation(p)
			);
		},

		perpendicularLine: function(p, distance){
			var dx = this.p1.x - this.p2.x;
			var dy = this.p1.y - this.p2.y;

			var dist = Math.sqrt(dx*dx + dy*dy);
			dx = dx / dist;
			dy = dy / dist;

			return new SVG.math.Line(
				new SVG.math.Point(
					p.x + distance * dy,
					p.y - distance * dx
				),
				new SVG.math.Point(
					p.x - distance * dy,
					p.y + distance * dx
				)
			);
		}

	});

})();
///#source 1 1 /Scripts/svg/svg.parser.js
// svg.parser.js 0.1.0 - Copyright (c) 2014 Wout Fierens - Licensed under the MIT license
;(function() {

  SVG.parse = {
    // Convert attributes to an object
    attr: function(child) {
      var i
        , attrs = child.attributes || []
        , attr  = {}
      
      /* gather attributes */
      for (i = attrs.length - 1; i >= 0; i--)
        attr[attrs[i].nodeName] = attrs[i].nodeValue

      /* ensure stroke width where needed */
      if (typeof attr.stroke != 'undefined' && typeof attr['stroke-width'] == 'undefined')
        attr['stroke-width'] = 1
    
      return attr
    }

    // Convert transformations to an object
  , transform: function(transform) {
      var i, t, v
        , trans = {}
        , list  = (transform || '').match(/[A-Za-z]+\([^\)]+\)/g) || []
        , def   = SVG.defaults.trans()

      /* gather transformations */
      for (i = list.length - 1; i >= 0; i--) {
        /* parse transformation */
        t = list[i].match(/([A-Za-z]+)\(([^\)]+)\)/)
        v = (t[2] || '').replace(/^\s+/,'').replace(/,/g, ' ').replace(/\s+/g, ' ').split(' ')

        /* objectify transformation */
        switch(t[1]) {
          case 'matrix':
            trans.a         = SVG.regex.isNumber.test(v[0]) ? parseFloat(v[0]) : def.a
            trans.b         = parseFloat(v[1]) || def.b
            trans.c         = parseFloat(v[2]) || def.c
            trans.d         = SVG.regex.isNumber.test(v[3]) ? parseFloat(v[3]) : def.d
            trans.e         = parseFloat(v[4]) || def.e
            trans.f         = parseFloat(v[5]) || def.f
          break
          case 'rotate':
            trans.rotation  = parseFloat(v[0]) || def.rotation
            trans.cx        = parseFloat(v[1]) || def.cx
            trans.cy        = parseFloat(v[2]) || def.cy
          break
          case 'scale':
            trans.scaleX    = SVG.regex.isNumber.test(v[0]) ? parseFloat(v[0]) : def.scaleX
            trans.scaleY    = SVG.regex.isNumber.test(v[1]) ? parseFloat(v[1]) : def.scaleY
          break
          case 'skewX':
            trans.skewX     = parseFloat(v[0]) || def.skewX
          break
          case 'skewY':
            trans.skewY     = parseFloat(v[0]) || def.skewY
          break
          case 'translate':
            trans.x         = parseFloat(v[0]) || def.x
            trans.y         = parseFloat(v[1]) || def.y
          break
        }
      }

      return trans
    }
  }

})();
///#source 1 1 /Scripts/svg/svg.path.js
/** svg.path.js - v0.6.0 - 2014-08-15
 * http://otm.github.io/svg.path.js/
 * Copyright (c) 2014 Nils Lagerkvist; Licensed under the  MIT license /
 */
(function() {

	var slice = Function.prototype.call.bind(Array.prototype.slice);

	SVG.extend(SVG.Path, {
		M: function(p){
			p = (arguments.length === 1) ? [p.x, p.y] : slice(arguments);

			this.addSegment('M', p, this._redrawEnabled);

			if (this._segments.length === 1){
				return this.plot('M' + p[0] + ' ' + p[1]);
			}

			return this;
		},
		m: function(p){
			p = (arguments.length === 1) ? [p.x, p.y] : slice(arguments);

			this.addSegment('m', p, this._redrawEnabled);

			if (this._segments.length === 1){
				return this.plot('m' + p[0] + ' ' + p[1]);
			}

			return this;
		},
		// TODO: Solve
		L: function(p) {
			p = (arguments.length === 1) ? [p.x, p.y] : slice(arguments);

			return this.addSegment('L', p, this._redrawEnabled);
		},
		l: function(p) {
			p = (arguments.length === 1) ? [p.x, p.y] : slice(arguments);

			return this.addSegment('l', p, this._redrawEnabled);
		},
		H: function(x){
			return this.addSegment('H', [x], this._redrawEnabled);
		},
		h: function(x){
			return this.addSegment('h', [x], this._redrawEnabled);
		},
		V: function(y){
			return this.addSegment('V', [y], this._redrawEnabled);
		},
		v: function(y){
			return this.addSegment('v', [y], this._redrawEnabled);
		},
		C: function(p1, p2, p){
			p = (arguments.length === 3) ? [p1.x, p1.y, p2.x, p2.y, p.x, p.y] : slice(arguments);

			return this.addSegment('C', p, this._redrawEnabled);
		},
		c: function(p1, p2, p){
			p = (arguments.length === 3) ? [p1.x, p1.y, p2.x, p2.y, p.x, p.y] : slice(arguments);

			return this.addSegment('c', p, this._redrawEnabled);
		},
		S: function(p2, p){
			p = (arguments.length === 2) ? [p2.x, p2.y, p.x, p.y] : slice(arguments);

			return this.addSegment('S', p, this._redrawEnabled);
		},
		s: function(p2, p){
			p = (arguments.length === 2) ? [p2.x, p2.y, p.x, p.y] : slice(arguments);

			return this.addSegment('s', p, this._redrawEnabled);
		},
		// Q x1 y1, x y
		Q: function(p1, p){
			p = (arguments.length === 2) ? [p1.x, p1.y, p.x, p.y] : slice(arguments);

			return this.addSegment('Q', p, this._redrawEnabled);
		},
		q: function(p1, p){
			p = (arguments.length === 2) ? [p1.x, p1.y, p.x, p.y] : slice(arguments);

			return this.addSegment('q', p, this._redrawEnabled);
		},
		T: function(p){
			p = (arguments.length === 1) ? [p.x, p.y] : slice(arguments);

			return this.addSegment('T', p, this._redrawEnabled);
		},
		t: function(p){
			p = (arguments.length === 1) ? [p.x, p.y] : slice(arguments);

			return this.addSegment('t', p, this._redrawEnabled);
		},
		A: function(rx, ry, xAxisRotation, largeArcFlag, sweepFlag, p){
			p = (arguments.length === 6) ? [rx, ry, xAxisRotation, largeArcFlag, sweepFlag, p.x, p.y] : slice(arguments);

			return this.addSegment('A', p, this._redrawEnabled);
		},
		a: function(rx, ry, xAxisRotation, largeArcFlag, sweepFlag, p){
			p = (arguments.length === 6) ? [rx, ry, xAxisRotation, largeArcFlag, sweepFlag, p.x, p.y] : slice(arguments);

			return this.addSegment('a', p, this._redrawEnabled);
		},
		Z: function(){
			return this.addSegment('Z', [], this._redrawEnabled);
		},
		// TODO: Add check that first element is moveto
		addSegment: function(movement, coordinates, redraw){
			var segment = {
				type: movement,
				coords: coordinates
			};

			if (!this._segments){
				this._segments = [];
			}

			this._segments.push(segment);

			if (redraw !== false){
				this._drawSegment(segment);
			}

			return this;
		},
		clear: function(){
			if (this._segments){
				this._segments.length = 0;
			}
			this._lastSegment = null;
			return this.plot();
		},
		getSegmentCount: function(){
			return this._segments.length;
		},
		getSegment: function(index){
			return this._segments[index];
		},
		removeSegment: function(index){
			this._segments.splice(index, 1);
			return this.redraw();
		},
		replaceSegment: function(index, segment){
			this._segments.splice(index, 1, segment);
			return this.redraw();
		},
		/**
		 * Easing:
		 *	<>: ease in and out
		 *	>: ease out
		 *	<: ease in
		 *	-: linear
		 *	=: external control
		 *	a function
		 */
		drawAnimated: function(options){
			options = options || {};
			options.duration = options.duration || '1000';
			options.easing = options.easing || '<>';
			options.delay = options.delay || 0;
			
			var length = this.length();

			this.stroke({
				width:         2,
				dasharray:     length + ' ' + length,
				dashoffset:    length
			});

			var fx = this.animate(options.duration, options.easing, options.delay);

			fx.stroke({
				dashoffset: 0
			});
			
			return this;
		},
		update: function(redraw){
			if (redraw === true)
				this._redrawEnabled = false;

			if (redraw === false)
				this._redrawEnabled = true;

			return !!this._redrawEnabled;
		},
		redraw: function(){
			// reset
			this._lastSegment = null;
			this.attr('d', '');

			return this._drawSegment(this._segments);
		},
		_drawSegment: function(segment){
			var str = '', lastSegment = this._lastSegment;

			if (!Array.isArray(segment)){
				segment = [segment];
			}

			for (var i = 0; i < segment.length; i += 1){
				if (lastSegment === segment[i].type){
					str += ' ' + segment[i].coords.join(' ');
				}
				else{
					str += ' ' + segment[i].type + segment[i].coords.join(' ');
				}
				lastSegment = segment[i].type;
			}

			this._lastSegment = lastSegment;	

			return this.attr('d', (this.attr('d') || '') + str);
		}
	});

}).call(this);

///#source 1 1 /Scripts/svg/svg.draggy.js
(function() {
    SVG.extend(SVG.Element, {
        /**
         * draggy
         * Makes an element draggable.
         *
         * @name draggy
         * @function
         * @param {Object|Function} constraint An object containing the
         * constraint values or a function which gets the `x` and `y` values
         * and returns a boolean or an object containing the `x` and `y`
         * boolean values.`false` skips moving while `true` allows it.
         * @return {SVG} The SVG element.
         */
        draggy: function (constraint) {

            var start
              , drag
              , end
              , element = this
              ;

            // Remove draggable if already present
            if (typeof this.fixed === "function") {
                this.fixed();
            }

            // Ensure constraint object
            constraint = constraint || {};

            // Start dragging
            start = function(event) {
                var parent = this.parent._parent(SVG.Nested) || this._parent(SVG.Doc);
                event = event || window.event;

                // Invoke any callbacks
                if (element.beforedrag) {
                    element.beforedrag(event);
                }

                // Get element bounding box
                var box = element.bbox();

                if (element instanceof SVG.G) {
                    box.x = element.x();
                    box.y = element.y();
                } else if (element instanceof SVG.Nested) {
                    box = {
                        x: element.x()
                      , y: element.y()
                      , width: element.width()
                      , height: element.height()
                    };
                }

                // Store event
                element.startEvent = event;

                // Store start position
                element.startPosition = {
                    x: box.x
                  , y: box.y
                  , width: box.width
                  , height: box.height
                  , zoom: parent.viewbox().zoom
                  , rotation: element.transform("rotation") * Math.PI / 180
                };

                // Add while and end events to window
                SVG.on(window, "mousemove", drag);
                SVG.on(window, "touchmove", drag);

                SVG.on(window, "mouseup", end);
                SVG.on(window, "touchend", end);

                // Invoke any callbacks
                element.node.dispatchEvent(new CustomEvent("dragstart", {
                    detail: {
                        event: event
                      , delta: {
                            x: 0
                          , y: 0
                        }
                    }
                }));

                // Prevent selection dragging
                if (event.preventDefault) {
                    event.preventDefault();
                } else {
                    event.returnValue = false;
                }
            };

            function elmZoom(elm) {
                if (!elm || typeof elm.transform !== "function") { return { x: 1, y: 1 }; }
                var p = elm.parent;
                var t = elm.transform();
                pz = {};
                var pz = elmZoom(p);
                return {
                    x: t.scaleX * pz.x
                  , y: t.scaleY * pz.y
                };
            }

            // While dragging
            drag = function(event) {
                event = event || window.event;

                if (element.startEvent) {
                    // Calculate move position
                    var x
                      , y
                      , rotation = element.startPosition.rotation
                      , width = element.startPosition.width
                      , height = element.startPosition.height
                      , delta = {
                            x: event.pageX - element.startEvent.pageX
                          , y: event.pageY - element.startEvent.pageY
                        }
                      ;

                    if (/^touchstart|touchmove$/.test(event.type)) {
                        delta.x = event.touches[0].pageX - element.startEvent.touches[0].pageX;
                        delta.y = event.touches[0].pageY - element.startEvent.touches[0].pageY;
                    } else if(/^click|mousedown|mousemove$/.test(event.type)) {
                        delta.x = event.pageX - element.startEvent.pageX;
                        delta.y = event.pageY - element.startEvent.pageY;
                    }

                    delta.scale = elmZoom(element);

                    x = element.startPosition.x + (delta.x * Math.cos(rotation) + delta.y * Math.sin(rotation)) / Math.pow(delta.scale.x, 2);
                    y = element.startPosition.y + (delta.y * Math.cos(rotation) + delta.x * Math.sin(-rotation)) / Math.pow(delta.scale.y, 2);

                    // Move the element to its new position, if possible by constraint
                    if (typeof constraint === "function") {
                        var coord = constraint(x, y);
                        if (typeof coord === "object") {
                            if (typeof coord.x !== "boolean" || coord.x) {
                                x = typeof coord.x === "number" ? coord.x : x;
                                element.x(x);
                            } else {
                                x = element.x();
                            }

                            if (typeof coord.y !== "boolean" || coord.y) {
                                y = typeof coord.y === "number" ? coord.y : y;
                                element.y(y);
                            } else {
                                y = element.y();
                            }
                        } else if (typeof coord === "boolean" && coord) {
                            element.move(x, y);
                        } else {
                            x = element.x();
                            y = element.y();
                        }
                    } else if (typeof constraint === "object") {
                        // Keep element within constrained box
                        if (constraint.minX !== null && x < constraint.minX) {
                            x = constraint.minX;
                        } else if (constraint.maxX !== null && x > constraint.maxX - width) {
                            x = constraint.maxX - width;
                        }

                        if (constraint.minY !== null && y < constraint.minY) {
                            y = constraint.minY;
                        } else if (constraint.maxY !== null && y > constraint.maxY - height) {
                            y = constraint.maxY - height;
                        }

                        element.move(x, y);
                    }

                    // Calculate the total movement
                    delta.movedX = x - element.startPosition.x;
                    delta.movedY = y - element.startPosition.y;

                    // Invoke any callbacks
                    element.node.dispatchEvent(new CustomEvent("dragmove", {
                        detail: {
                            delta: delta
                          , event: event
                        }
                    }));
                }
            };

            // When dragging ends
            end = function(event) {
                event = event || window.event;

                // Calculate move position
                var delta = {
                    x: event.pageX - element.startEvent.pageX
                  , y: event.pageY - element.startEvent.pageY
                  , zoom: element.startPosition.zoom
                };

                // Reset store
                element.startEvent = null;
                element.startPosition = null;

                // Remove while and end events to window
                SVG.off(window, "mousemove", drag);
                SVG.off(window, "touchmove", drag);
                SVG.off(window, "mouseup", end);
                SVG.off(window, "touchend", end);

                // Invoke any callbacks
                element.node.dispatchEvent(new CustomEvent("dragend", {
                    detail: {
                        delta: {
                            x: 0
                          , y: 0
                        }
                      , event: event
                    }
                }));
            };

            // Bind mousedown event
            element.on("mousedown", start);
            element.on("touchstart", start);

            // Disable draggable
            element.fixed = function() {
                element.off("mousedown", start);
                element.off("touchstart", start);

                SVG.off(window, "mousemove", drag);
                SVG.off(window, "touchmove", drag);
                SVG.off(window, "mouseup", end);
                SVG.off(window, "touchend", end);

                start = drag = end = null;

                return element;
            };

            return this;
        }
    });
}).call(this);

///#source 1 1 /Scripts/svg/svg.connectable.js
/*!
 * SVG.js Connectable Plugin
 * =========================
 *
 * A JavaScript library for connecting SVG things.
 * Created with <3 and JavaScript by the jillix developers.
 *
 * svg.connectable.js 1.0.1
 * Licensed under the MIT license.
 * */
;(function() {

    var container = null;
    var markers = null;

    /**
     * connectable
     * Connects two elements.
     *
     * @name connectable
     * @function
     * @param {Object} options An object containing the following fields:
     *
     *  - `container` (SVGElement): The line elements container.
     *  - `markers` (SVGElement): The marker elements container.
     *
     * @param {SVGElement} elmTarget The target SVG element.
     * @return {Object} The connectable object containing:
     *
     *  - `source` (SVGElement): The source element.
     *  - `target` (SVGElement): The target element.
     *  - `line` (SVGElement): The line element.
     *  - `marker` (SVGElement): The marker element.
     *  - `padEllipe` (Boolean): If `true`, the line coordinates will be placed with a padding.
     *  - [`computeLineCoordinates` (Function)](#computelinecoordinatescon)
     *  - [`update` (Function)](#update)
     *  - [`setLineColor` (Function)](#setlinecolorcolor-c)
     */
    function connectable(options, elmTarget) {

        var con = {};

        if (elmTarget === undefined) {
            elmTarget = options;
            options = {};
        }

        container = options.container || container;
        var elmSource = this;
        markers = options.markers || markers;

        var marker = markers.marker(10, 10);
        var markerId = "triangle-" + Math.random().toString(16);
        var line = container.line().attr("marker-end", "url(#" + markerId + ")");

        marker.attr({
            id: markerId,
            viewBox: "0 0 10 10",
            refX: "0",
            refY: "5",
            markerUnits: "strokeWidth",
            markerWidth: "4",
            markerHeight: "5"
        });

        marker.path().attr({
            d: "M 0 0 L 10 5 L 0 10 z"
        });

        // Source and target positions
        var sPos = {};
        var tPos = {};

        // Append the SVG elements
        con.source = elmSource;
        con.target = elmTarget;
        con.line = line;
        con.marker = marker;

        /**
         * computeLineCoordinates
         * The function that computes the new coordinates.
         * It can be overriden with a custom function.
         *
         * @name computeLineCoordinates
         * @function
         * @param {Connectable} con The connectable instance.
         * @return {Object} An object containing the `x1`, `x2`, `y1` and `y2` coordinates.
         */
        con.computeLineCoordinates = function (con) {

            var sPos = con.source.bbox();
            var tPos = con.target.bbox();

            var x1 = sPos.x + sPos.width / 2;
            var y1 = sPos.y + sPos.height / 2;
            var x2 = tPos.x + tPos.width / 2;
            var y2 = tPos.y + tPos.height / 2;

            return {
                x1: x1,
                y1: y1,
                x2: x2,
                y2: y2
            };
        };

        if (options.padEllipse) {
            con.computeLineCoordinates = function (con) {
                var sPos = con.source.transform();
                var tPos = con.target.transform();

                // Get ellipse radiuses
                var xR1 = parseFloat(con.source.node.querySelector("ellipse").getAttribute("rx"));
                var yR1 = parseFloat(con.source.node.querySelector("ellipse").getAttribute("ry"));

                var xR2 = parseFloat(con.target.node.querySelector("ellipse").getAttribute("rx"));
                var yR2 = parseFloat(con.target.node.querySelector("ellipse").getAttribute("ry"));

                // Get centers
                var sx = sPos.x + xR1 / 2;
                var sy = sPos.y + yR1 / 2;

                var tx = tPos.x + xR2 / 2;
                var ty = tPos.y + yR2 / 2;

                // Calculate distance from source center to target center
                var dx = tx - sx;
                var dy = ty - sy;
                var d = Math.sqrt(dx * dx + dy * dy);

                // Construct unit vector between centers
                var ux = dx / d;
                var uy = dy / d;

                // Point on source circle
                var x1 = sx + xR1 * ux;
                var y1 = sy + yR1 * uy;

                // Point on target circle
                var x2 = sx + (d - xR2 - 5) * ux;
                var y2 = sy + (d - yR2 - 5) * uy;

                return {
                    x1: x1 + xR1 / 2,
                    y1: y1 + yR1 / 2,
                    x2: x2 + xR2 / 2,
                    y2: y2 + yR2 / 2
                };
            };
        }

        elmSource.cons = elmSource.cons || [];
        elmSource.cons.push(con);

        /**
         * update
         * Updates the line coordinates.
         *
         * @name update
         * @function
         * @return {undefined}
         */
        con.update = function () {
            line.attr(con.computeLineCoordinates(con));
        };
        con.update();
        elmSource.on("dragmove", con.update);
        elmTarget.on("dragmove", con.update);

        /**
         * setLineColor
         * Sets the line color.
         *
         * @name setLineColor
         * @function
         * @param {String} color The new color.
         * @param {Connectable} c The connectable instance.
         * @return {undefined}
         */
        con.setLineColor = function (color, c) {
            c = c || this;
            c.line.stroke(color);
            c.marker.fill(color);
        };

        return con;
    }

    SVG.extend(SVG.Element, {
        connectable: connectable
    });
}).call(this);

///#source 1 1 /Scripts/svg/svg.absorb.js
// svg.absorb.js 0.1.2 - Copyright (c) 2014 Wout Fierens - Licensed under the MIT license
;(function() {

  SVG.Absorbee = SVG.invent({
    // Create js wrapper
    create: function(node) {
      this.node = node
      this.type = node.localName
      this.node.instance = this
    }

    // Inherit from SVG.Element
  , inherit: SVG.Element

    // How the element is constructed
  , construct: {
      // Add absorb method to container elements
      absorb: function(raw) {
        if (typeof raw === 'string') {
          /* create temporary div to receive svg content */
          var i
            , well = document.createElement('div')

          /* strip away newlines and properly close tags */
          raw = raw
            .replace(/\n/, '')
            .replace(/<(\w+)([^<]+?)\/>/g, '<$1$2></$1>')

          /* ensure SVG wrapper for correct element type cating */
          well.innerHTML = '<svg version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">' + raw + '</svg>'

          /* transplant content from well to target */
          for (i = well.firstChild.childNodes.length - 1; i >= 0; i--)
            if (well.firstChild.childNodes[i].nodeType == 1)
              this.add(new SVG.Absorbee(well.firstChild.childNodes[i]), 0)
          
          /* mark temporary div for garbage collection */
          well = null

        } else {
          this.add(new SVG.Absorbee(raw))
        }
        
        return this
      }
    }

  })

}).call(this);
///#source 1 1 /Scripts/svg/svg.draw.js
/*! svg.draw.js - v1.0.0 - 2015-05-25
* https://github.com/Fuzzyma/svg.draw.js
* Copyright (c) 2015 Ulrich-Matthias Schäfer; Licensed MIT */
;(function () {

    // Calculates the offset of an element
    function offset(el) {
        var x = 0, y = 0;

        if ('doc' in el) {
            var box = el.bbox();
            x = box.x;
            y = box.y;
            el = el.doc().parent;
        }

        while (el.nodeName.toUpperCase() !== 'BODY') {
            x += el.offsetLeft;
            y += el.offsetTop;
            el = el.offsetParent;
        }

        return {x: x, y: y};
    }

    function PaintHandler(el, event, options) {

        var _this = this;

        el.remember('_paintHandler', this);

        this.el = el;
        this.parent = el.parent._parent(SVG.Nested) || el._parent(SVG.Doc);
        this.set = this.parent.set();
        this.parameters = {};
        this.event = event;
        this.plugin = this.getPlugin();
        this.options = {};

        // Merge options and defaults
        for (var i in this.el.draw.defaults) {
            this.options[i] = this.el.draw.defaults[i];
            if (typeof options[i] !== 'undefined') {
                this.options[i] = options[i];
            }
        }
        // When we got an event, we use this for start, otherwise we use the click-event as default
        if (!event) {
            this.parent.on('click.draw', function (e) {
                _this.start(e || window.event);
            });
        }
        /*else {
         this.start(event);
         }*/

    }

    PaintHandler.prototype.start = function (event) {

        this.parameters = { x: event.pageX, y: event.pageY, offset: offset(this.parent) };

        var draw = {}, element = this.el, _this = this;

        // For every element-type we need a different function to calculate the parameters
        // and of course we need different start-conditions
        switch (element.type) {

            // Rectangle
            case 'rect':

                // Set the default parameters for a rectangle and draw it
                draw = { x: event.pageX, y: event.pageY, height: 1, width: 1 };
                element.attr(draw);

                // assign the calc-function which calculates position, width and height
                this.calc = function (event) {
                    draw.x = this.parameters.x;
                    draw.y = this.parameters.y;
                    draw.height = event.pageY - draw.y;
                    draw.width = event.pageX - draw.x;

                    // Correct the Position
                    // the cursor-position is absolute to the html-document but our parent-element is not
                    draw.x -= this.parameters.offset.x;
                    draw.y -= this.parameters.offset.y;

                    // Snap the params to the grid we specified
                    this.snapToGrid(draw);

                    // When width is less than one, we have to draw to the left
                    // which means we have to move the start-point to the left
                    if (draw.width < 1) {
                        draw.x = draw.x + draw.width;
                        draw.width = -draw.width;
                    }

                    // ...same with height
                    if (draw.height < 1) {
                        draw.y = draw.y + draw.height;
                        draw.height = -draw.height;
                    }

                    // draw the element
                    element.attr(draw);
                };
                break;

            // Line
            case 'line':
                // s.a.
                this.calc = function (event) {
                    draw.x1 = this.parameters.x - this.parameters.offset.x;
                    draw.y1 = this.parameters.y - this.parameters.offset.y;
                    draw.x2 = event.pageX - this.parameters.offset.x;
                    draw.y2 = event.pageY - this.parameters.offset.y;
                    this.snapToGrid(draw);
                    element.attr(draw);
                };
                break;

            // Polygon and Polyline
            case 'polyline':
            case 'polygon':

                // When we draw a polygon, we immediately need 2 points.
                // One start-point and one point at the mouse-position
                element.array.value[0] = this.snapToGrid([event.pageX - this.parameters.offset.x, event.pageY - this.parameters.offset.y]);
                element.array.value[1] = this.snapToGrid([event.pageX - this.parameters.offset.x, event.pageY - this.parameters.offset.y]);
                element.plot(element.array);

                // We draw little circles around each point
                // This is absolutely not needed and maybe removed in a later release
                this.drawCircles(element.array.value);

                // The calc-function sets the position of the last point to the mouse-position (with offset ofc)
                this.calc = function (event) {
                    element.array.value.pop();
                    if (event) {
                        element.array.value.push(this.snapToGrid([event.pageX - this.parameters.offset.x, event.pageY - this.parameters.offset.y]));
                    }
                    element.plot(element.array);
                };
                break;

            // Circles and Ellipsoid
            case 'ellipse':

                // We start with a circle with radius 1 at the position of the cursor
                draw = { cx: event.pageX, cy: event.pageY, rx: 1, ry: 1 };
                this.snapToGrid(draw);
                element.attr(draw);

                // When using the cursor-position as radius, we can only draw circles
                if (this.options.useRadius) {
                    this.calc = function (event) {
                        draw.cx = this.parameters.x - this.parameters.offset.x;
                        draw.cy = this.parameters.y - this.parameters.offset.y;

                        // calculating the radius
                        draw.rx = draw.ry = Math.sqrt(
                            (event.pageX - this.parameters.x) * (event.pageX - this.parameters.x) +
                                (event.pageY - this.parameters.y) * (event.pageY - this.parameters.y)
                        );
                        this.snapToGrid(draw);
                        element.attr(draw);
                    };
                    // otherwise we threat the cursor-position as width and height of the circle/ellipse
                } else {
                    this.calc = function (event) {
                        draw.cx = this.parameters.x - this.parameters.offset.x;
                        draw.cy = this.parameters.y - this.parameters.offset.y;
                        draw.rx = Math.abs(event.pageX - this.parameters.x);
                        draw.ry = Math.abs(event.pageY - this.parameters.y);
                        this.snapToGrid(draw);
                        element.attr(draw);
                    };
                }
                break;
            // unknown type, try to find a plugin for that type
            default:
                if (this.plugin) {
                    this.plugin.init.call(this, event);
                    this.calc = this.plugin.calc;
                } else {
                    return;
                }
                break;
        }

        // Fire our `drawstart`-event. We send the offset-corrected cursor-position along
        element.fire('drawstart', [event.pageX - this.parameters.offset.x, event.pageY - this.parameters.offset.y]);

        // We need to bind the update-function to the mousemove event to keep track of the cursor
        SVG.on(window, 'mousemove.draw', function (e) {
            _this.update(e || window.event);
        });

        this.start = this.point;


    };

    // This function draws a point if the element is a polyline or polygon
    // Otherwise it will just stop drawing the shape cause we are done    
    PaintHandler.prototype.point = function (event) {

        if (this.plugin && this.plugin.point) {
            this.plugin.point.call(this, event);
            return;
        }

        if (this.el.type.indexOf('poly') > -1) {
            // Add the new Point to the point-array
            var newPoint = [event.pageX - this.parameters.offset.x, event.pageY - this.parameters.offset.y];
            this.el.array.value.push(this.snapToGrid(newPoint));
            this.el.plot(this.el.array);
            this.drawCircles(this.el.array.value);

            // Fire the `drawpoint`-event, which holds the coords of the new Point
            this.el.fire('drawpoint', newPoint);
            return;
        }

        // We are done, if the element is no polyline or polygon
        this.stop(event);
    };


    // The stop-function does the cleanup work
    PaintHandler.prototype.stop = function (event) {
        if (event) {
            this.update(event);
        }

        // Remove all circles
        this.set.each(function () {
            this.remove();
        });

        // Unbind from all events
        SVG.off(window, 'mousemove.draw');
        this.parent.off('click.draw');

        // remove Refernce to PaintHandler
        this.el.forget('_paintHandler');

        // overwrite draw-function since we never need it again for this element
        this.el.draw = function () {
        };

        // Fire the `drawstop`-event
        this.el.fire('drawstop');
    };

    // Updates the element while moving the cursor
    PaintHandler.prototype.update = function (event) {

        // Because we are nice - we give you coords we nowhere need in this function anymore
        var updateParams = [ event.pageX - this.parameters.offset.x,
            event.pageY - this.parameters.offset.y ];

        // Call the calc-function which calculates the new position and size
        this.calc(event);

        // Fire the `drawupdate`-event
        this.el.fire('drawupdate', updateParams);
    };

    // Called from outside. Finishs a poly-element
    PaintHandler.prototype.done = function () {
        this.calc();
        this.stop();

        this.el.fire('drawdone');
    };

    // Called from outside. Cancels a poly-element
    PaintHandler.prototype.cancel = function () {
        // stop drawing and remove the element
        this.stop();
        this.el.remove();

        this.el.fire('drawcancel');
    };

    // Draws circles at the position of the edges from polygon and polyline
    PaintHandler.prototype.drawCircles = function (array) {
        this.set.each(function () {
            this.remove();
        });
        this.set.clear();
        for (var i = 0; i < array.length; ++i) {
            this.set.add(this.parent.circle(5).stroke({width: 1}).fill('#ccc').center(array[i][0], array[i][1]));
        }
    };

    // Calculate the corrected position when using `snapToGrid`
    PaintHandler.prototype.snapToGrid = function (draw) {

        var temp = null;

        // An array was given. Loop through every element
        if (draw.length) {
            temp = [draw[0] % this.options.snapToGrid, draw[1] % this.options.snapToGrid];
            draw[0] -= temp[0] < this.options.snapToGrid / 2 ? temp[0] : temp[0] - this.options.snapToGrid;
            draw[1] -= temp[1] < this.options.snapToGrid / 2 ? temp[1] : temp[1] - this.options.snapToGrid;
            return draw;
        }

        // Properties of element were given. Snap them all
        for (var i in draw) {
            temp = draw[i] % this.options.snapToGrid;
            draw[i] -= (temp < this.options.snapToGrid / 2 ? temp : temp - this.options.snapToGrid) + (temp < 0 ? this.options.snapToGrid : 0);
        }

        return draw;
    };

    PaintHandler.prototype.param = function (key, value) {
        this.options[key] = value === null ? this.el.draw.defaults[key] : value;
    };

    // Returns the plugin
    PaintHandler.prototype.getPlugin = function () {
        return this.el.draw.plugins[this.el.type];
    };

    SVG.extend(SVG.Element, {
        // Draw element with mouse
        draw: function (event, options, value) {

            // sort the parameters
            if (!(event instanceof Event || typeof event === 'string')) {
                options = event;
                event = null;
            }

            // get the old Handler or create a new one from event and options
            var paintHandler = this.remember('_paintHandler') || new PaintHandler(this, event, options || {});

            // When we got an event we have to start/continue drawing
            if (event instanceof Event) {
                paintHandler['start'](event);
            }

            // if event is located in our PaintHandler we handle it as method
            if (paintHandler[event]) {
                paintHandler[event](options, value);
            }

            return this;
        }

    });

    // Default values. Can be changed for the whole project if needed
    SVG.Element.prototype.draw.defaults = {
        useRadius: false,    // If true, we draw the circle using the cursor as radius rather than using it for width and height of the circle
        snapToGrid: 1        // Snaps to a grid of `snapToGrid` px
    };

    // Container for all types not specified here
    SVG.Element.prototype.draw.plugins = {};

}).call(this);
///#source 1 1 /Scripts/svg/svg.easing.js
// svg.easing.js 0.3 - Copyright (c) 2013 Wout Fierens - Licensed under the MIT license
// Based on Easing Equations (c) 2003 [Robert Penner](http://www.robertpenner.com/), all rights reserved.

SVG.easing = {
  
  quadIn: function(pos) {
    return Math.pow(pos, 2)
  }

, quadOut: function(pos) {
    return -(Math.pow((pos - 1), 2) - 1)
  }

, quadInOut: function(pos) {
    if ((pos /= 0.5) < 1) return 0.5 * Math.pow(pos, 2)
    return -0.5 * ((pos -= 2) * pos - 2)
  }

, cubicIn: function(pos) {
    return Math.pow(pos, 3)
  }

, cubicOut: function(pos) {
    return (Math.pow((pos - 1), 3) + 1)
  }

, cubicInOut: function(pos) {
    if ((pos /= 0.5) < 1) return 0.5 * Math.pow(pos,3)
    return 0.5 * (Math.pow((pos - 2), 3) + 2)
  }

, quartIn: function(pos) {
    return Math.pow(pos, 4)
  }

, quartOut: function(pos) {
    return -(Math.pow((pos-1), 4) -1)
  }

, quartInOut: function(pos) {
    if ((pos /= 0.5) < 1) return 0.5 * Math.pow(pos, 4)
    return -0.5 * ((pos -= 2) * Math.pow(pos, 3) - 2)
  }

, quintIn: function(pos) {
    return Math.pow(pos, 5)
  }

, quintOut: function(pos) {
    return (Math.pow((pos-1), 5) +1)
  }

, quintInOut: function(pos) {
    if ((pos /= 0.5) < 1) return 0.5 * Math.pow(pos, 5)
    return 0.5 * (Math.pow((pos - 2), 5) + 2)
  }

, sineIn: function(pos) {
    return -Math.cos(pos * (Math.PI / 2)) + 1
  }

, sineOut: function(pos) {
    return Math.sin(pos * (Math.PI / 2))
  }

, sineInOut: function(pos) {
    return (-.5 * (Math.cos(Math.PI * pos) -1))
  }

, expoIn: function(pos) {
    return (pos==0) ? 0 : Math.pow(2, 10 * (pos - 1))
  }

, expoOut: function(pos) {
    return (pos==1) ? 1 : -Math.pow(2, -10 * pos) + 1
  }

, expoInOut: function(pos) {
    if(pos==0) return 0
    if(pos==1) return 1
    if((pos/=0.5) < 1) return 0.5 * Math.pow(2,10 * (pos-1))
    return 0.5 * (-Math.pow(2, -10 * --pos) + 2)
  }

, circIn: function(pos) {
    return -(Math.sqrt(1 - (pos*pos)) - 1)
  }

, circOut: function(pos) {
    return Math.sqrt(1 - Math.pow((pos-1), 2))
  }

, circInOut: function(pos) {
    if((pos/=0.5) < 1) return -0.5 * (Math.sqrt(1 - pos*pos) - 1)
    return 0.5 * (Math.sqrt(1 - (pos-=2)*pos) + 1)
  }

, backIn: function (pos) {
    var s = 1.70158
    return pos * pos * ((s + 1) * pos - s)
  }
  
, backOut: function (pos) {
    pos = pos - 1
    var s = 1.70158
    return pos * pos * ((s + 1) * pos + s) + 1
  }

, backInOut: function (pos) {
    var s = 1.70158
    if((pos/=0.5) < 1) return 0.5*(pos*pos*(((s*=(1.525))+1)*pos -s))
    return 0.5*((pos-=2)*pos*(((s*=(1.525))+1)*pos +s) +2)
  }

, swingFromTo: function(pos) {
    var s = 1.70158
    return ((pos/=0.5) < 1) ? 0.5*(pos*pos*(((s*=(1.525))+1)*pos - s)) :
    0.5*((pos-=2)*pos*(((s*=(1.525))+1)*pos + s) + 2)
  }

, swingFrom: function(pos) {
    var s = 1.70158
    return pos*pos*((s+1)*pos - s)
  }

, swingTo: function(pos) {
    var s = 1.70158
    return (pos-=1)*pos*((s+1)*pos + s) + 1
  }

, bounce: function(pos) {
    var s = 7.5625,
        p = 2.75,
        l
        
    if (pos < (1 / p)) {
      l = s * pos * pos
    } else {
      if (pos < (2 / p)) {
        pos -= (1.5 / p)
        l = s * pos * pos + .75
      } else {
        if (pos < (2.5 / p)) {
          pos -= (2.25 / p)
          l = s * pos * pos + .9375
        } else {
          pos -= (2.625 / p)
          l = s * pos * pos + .984375
        }
      }
    }
    return l
  }

, bounceOut: function(pos){
    if ((pos) < (1/2.75)) {
      return (7.5625*pos*pos)
    } else if (pos < (2/2.75)) {
      return (7.5625*(pos-=(1.5/2.75))*pos + .75)
    } else if (pos < (2.5/2.75)) {
      return (7.5625*(pos-=(2.25/2.75))*pos + .9375)
    } else {
      return (7.5625*(pos-=(2.625/2.75))*pos + .984375)
    }
  }

, elastic: function(pos) {
    if (pos == !!pos) return pos
    return Math.pow(2, -10 * pos) * Math.sin((pos - 0.075) * (2 * Math.PI) / .3) + 1
  }

}















///#source 1 1 /Scripts/svg/svg.export.js
// svg.export.js 0.1.1 - Copyright (c) 2014 Wout Fierens - Licensed under the MIT license

;(function() {

  // Add export method to SVG.Element 
  SVG.extend(SVG.Element, {
    // Build node string
    exportSvg: function(options, level) {
      var i, il, width, height, well, clone
        , name = this.node.nodeName
        , node = ''
      
      /* ensure options */
      options = options || {}
      
      if (options.exclude == null || !options.exclude.call(this)) {
        /* ensure defaults */
        options = options || {}
        level = level || 0
        
        /* set context */
        if (this instanceof SVG.Doc) {
          /* define doctype */
          node += whitespaced('<?xml version="1.0" encoding="UTF-8"?>', options.whitespace, level)
          
          /* store current width and height */
          width  = this.attr('width')
          height = this.attr('height')
          
          /* set required size */
          if (options.width)
            this.attr('width', options.width)
          if (options.height)
            this.attr('height', options.height)
        }
          
        /* open node */
        node += whitespaced('<' + name + this.attrToString() + '>', options.whitespace, level)
        
        /* reset size and add description */
        if (this instanceof SVG.Doc) {
          this.attr({
            width:  width
          , height: height
          })
          
          node += whitespaced('<desc>Created with svg.js [http://svgjs.com]</desc>', options.whitespace, level + 1)
          /* Add defs... */
					node += this.defs().exportSvg(options, level + 1);
        }
        
        /* add children */
        if (this instanceof SVG.Parent) {
          for (i = 0, il = this.children().length; i < il; i++) {
            if (SVG.Absorbee && this.children()[i] instanceof SVG.Absorbee) {
              clone = this.children()[i].node.cloneNode(true)
              well  = document.createElement('div')
              well.appendChild(clone)
              node += well.innerHTML
            } else {
              node += this.children()[i].exportSvg(options, level + 1)
            }
          }

        } else if (this instanceof SVG.Text || this instanceof SVG.TSpan) {
          for (i = 0, il = this.node.childNodes.length; i < il; i++)
            if (this.node.childNodes[i].instance instanceof SVG.TSpan)
              node += this.node.childNodes[i].instance.exportSvg(options, level + 1)
            else
              node += this.node.childNodes[i].nodeValue.replace(/&/g,'&amp;')

        } else if (SVG.ComponentTransferEffect && this instanceof SVG.ComponentTransferEffect) {
          this.rgb.each(function() {
            node += this.exportSvg(options, level + 1)
          })

        }
        
        /* close node */
        node += whitespaced('</' + name + '>', options.whitespace, level)
      }
      
      return node
    }
    // Set specific export attibutes
  , exportAttr: function(attr) {
      /* acts as getter */
      if (arguments.length == 0)
        return this.data('svg-export-attr')
      
      /* acts as setter */
      return this.data('svg-export-attr', attr)
    }
    // Convert attributes to string
  , attrToString: function() {
      var i, key, value
        , attr = []
        , data = this.exportAttr()
        , exportAttrs = this.attr()
      
      /* ensure data */
      if (typeof data == 'object')
        for (key in data)
          if (key != 'data-svg-export-attr')
            exportAttrs[key] = data[key]
      
      /* build list */
      for (key in exportAttrs) {
        value = exportAttrs[key]
        
        /* enfoce explicit xlink namespace */
        if (key == 'xlink') {
          key = 'xmlns:xlink'
        } else if (key == 'href') {
          if (!exportAttrs['xlink:href'])
            key = 'xlink:href'
        }

        /* normailse value */
        if (typeof value === 'string')
          value = value.replace(/"/g,"'")
        
        /* build value */
        if (key != 'data-svg-export-attr' && key != 'href') {
          if (key != 'stroke' || parseFloat(exportAttrs['stroke-width']) > 0)
            attr.push(key + '="' + value + '"')
        }
        
      }

      return attr.length ? ' ' + attr.join(' ') : ''
    }
    
  })
  
  /////////////
  // helpers
  /////////////

  // Whitespaced string
  function whitespaced(value, add, level) {
    if (add) {
      var whitespace = ''
        , space = add === true ? '  ' : add || ''
      
      /* build indentation */
      for (i = level - 1; i >= 0; i--)
        whitespace += space
      
      /* add whitespace */
      value = whitespace + value + '\n'
    }
    
    return value;
  }

}).call(this);

///#source 1 1 /Scripts/svg/svg.filter.js
// svg.filter.js 0.4 - Copyright (c) 2013-2014 Wout Fierens - Licensed under the MIT license
;(function() {

  // Main filter class
  SVG.Filter = function() {
    this.constructor.call(this, SVG.create('filter'))
  }

  // Inherit from SVG.Container
  SVG.Filter.prototype = new SVG.Parent

  //
  SVG.extend(SVG.Filter, {
    // Static strings
    source:           'SourceGraphic'
  , sourceAlpha:      'SourceAlpha'
  , background:       'BackgroundImage'
  , backgroundAlpha:  'BackgroundAlpha'
  , fill:             'FillPaint'
  , stroke:           'StrokePaint'
    // Custom put method for leaner code
  , put: function(element, i) {
      this.add(element, i)

      return element.attr({
        in:     this.source
      , result: element
      })
    }
    // Blend effect
  , blend: function(in1, in2, mode) {
      return this.put(new SVG.BlendEffect).attr({ in: in1, in2: in2, mode: mode || 'normal' })
    }
    // ColorMatrix effect
  , colorMatrix: function(type, values) {
      if (type == 'matrix')
        values = normaliseMatrix(values)

      return this.put(new SVG.ColorMatrixEffect).attr({
        type:   type
      , values: typeof values == 'undefined' ? null : values
      })
    }
    // ConvolveMatrix effect
  , convolveMatrix: function(matrix) {
      matrix = normaliseMatrix(matrix)

      return this.put(new SVG.ConvolveMatrixEffect).attr({
        order:        Math.sqrt(matrix.split(' ').length)
      , kernelMatrix: matrix
      })
    }
    // ComponentTransfer effect
  , componentTransfer: function(compontents) {
      var transfer = new SVG.ComponentTransferEffect

      /* create rgb set */
      transfer.rgb = new SVG.Set

      /* create components */
      ;(['r', 'g', 'b', 'a']).forEach(function(c) {
        /* create component */
        transfer[c] = new SVG['Func' + c.toUpperCase()]().attr('type', 'identity')

        /* store component in set */
        transfer.rgb.add(transfer[c])

        /* add component node */
        transfer.node.appendChild(transfer[c].node)
      })

      /* set components */
      if (compontents) {
        if (compontents.rgb) {
          /* set bundled components */
          ;(['r', 'g', 'b']).forEach(function(c) {
            transfer[c].attr(compontents.rgb)
          })

          delete compontents.rgb
        }
        
        /* set individual components */
        for (var c in compontents)
          transfer[c].attr(compontents[c])
      }
      
      return this.put(transfer) 
    }
    // Composite effect
  , composite: function(in1, in2, operator) {
      return this.put(new SVG.CompositeEffect).attr({ in: in1, in2: in2, operator: operator })
    }
    // Flood effect
  , flood: function(color) {
      return this.put(new SVG.FloodEffect).attr({ 'flood-color': color })
    }
    // Offset effect
  , offset: function(x, y) {
      return this.put(new SVG.OffsetEffect).attr({ dx: x, dy: y })
    }
    // Image effect
  , image: function(src) {
      return this.put(new SVG.ImageEffect).attr('href', src, SVG.xlink)
    }
    // Merge effect
  , merge: function() {
      // to be implemented
    }
    // Gaussian Blur effect
  , gaussianBlur: function() {
      return this.put(new SVG.GaussianBlurEffect).attr('stdDeviation', listString(Array.prototype.slice.call(arguments)))
    }
    // Default string value
  , toString: function() {
      return 'url(#' + this.attr('id') + ')'
    }

  })

  //
  SVG.extend(SVG.Defs, {
    // Define filter
    filter: function(block) {
      var filter = this.put(new SVG.Filter)
      
      /* invoke passed block */
      if (typeof block === 'function')
        block.call(filter, filter)
      
      return filter
    }
    
  })

  //
  SVG.extend(SVG.Container, {
    // Define filter on defs
    filter: function(block) {
      return this.defs().filter(block)
    }
    
  })

  //
  SVG.extend(SVG.Element, SVG.G, SVG.Nested, {
    // Create filter element in defs and store reference
    filter: function(block) {
      this.filterer = block instanceof SVG.Element ?
        block : this.doc().filter(block)

      this.attr('filter', this.filterer)

      return this.filterer
    }
    // Remove filter
  , unfilter: function(remove) {
      /* also remove the filter node */
      if (this.filterer && remove === true)
        this.filterer.remove()

      /* delete reference to filterer */
      delete this.filterer

      /* remove filter attribute */
      return this.attr('filter', null)
    }

  })

  // Create wrapping SVG.Effect class
  SVG.Effect = function() {}

  // Inherit from SVG.Element
  SVG.Effect.prototype = new SVG.Element

  SVG.extend(SVG.Effect, {
    // Set in attribute
    in: function(effect) {
      return this.attr('in', effect)
    }
    // Named result
  , result: function() {
      return this.attr('id') + 'Out'
    }
    // Stringification
  , toString: function() {
      return this.result()
    }

  })

  // Create all different effects
  var effects = [
      'blend'
    , 'colorMatrix'
    , 'componentTransfer'
    , 'composite'
    , 'convolveMatrix'
    , 'diffuseLighting'
    , 'displacementMap'
    , 'flood'
    , 'gaussianBlur'
    , 'image'
    , 'merge'
    , 'morphology'
    , 'offset'
    , 'specularLighting'
    , 'tile'
    , 'turbulence'
    , 'distantLight'
    , 'pointLight'
    , 'spotLight'
  ]

  effects.forEach(function(effect) {
    /* capitalize name */
    var name = effect.charAt(0).toUpperCase() + effect.slice(1)

    /* create class */
    SVG[name + 'Effect'] = function() {
      this.constructor.call(this, SVG.create('fe' + name))
    }

    /* inherit from SVG.Effect */
    SVG[name + 'Effect'].prototype = ['componentTransfer'].indexOf(name) > -1 ?
      new SVG.Parent : new SVG.Effect

    /* make all effects interchainable */
    effects.forEach(function(e) {

      SVG[name + 'Effect'].prototype[e] = function() {
        return this.parent[e].apply(this.parent, arguments).in(this)
      }

    })

  })

  // Create compontent functions
  ;(['r', 'g', 'b', 'a']).forEach(function(c) {
    /* create class */
    SVG['Func' + c.toUpperCase()] = function() {
      this.constructor.call(this, SVG.create('feFunc' + c.toUpperCase()))
    }

    /* inherit from SVG.Element */
    SVG['Func' + c.toUpperCase()].prototype = new SVG.Element

  })


  // Effect-specific extensions
  SVG.extend(SVG.FloodEffect, {
    // implement flood-color and flood-opacity
  })

  // Presets
  SVG.filter = {
    sepiatone:  [ .343, .669, .119, 0, 0 
                , .249, .626, .130, 0, 0
                , .172, .334, .111, 0, 0
                , .000, .000, .000, 1, 0 ]
  }

  // Helpers
  function normaliseMatrix(matrix) {
    /* convert possible array value to string */
    if (Array.isArray(matrix))
      matrix = new SVG.Array(matrix)

    /* ensure there are no leading, tailing or double spaces */
    return matrix.toString().replace(/^\s+/, '').replace(/\s+$/, '').replace(/\s+/g, ' ')
  }

  function listString(list) {
    if (!Array.isArray(list))
      return list

    for (var i = 0, l = list.length, s = []; i < l; i++)
      s.push(list[i])

    return s.join(' ')
  }

}).call(this);
///#source 1 1 /Scripts/svg/svg.import.js
// svg.import.js 1.0.1 - Copyright (c) 2014 Wout Fierens - Licensed under the MIT license
;(function() {

  // Convert nodes to svg.js elements
  function convertNodes(nodes, context, level, store, block) {
    var i, l, j, key, element, type, child, attr, transform, clips
    
    for (i = 0, l = nodes.length; i < l; i++) {
      child = nodes[i]
      attr  = {}
      clips = []
      element = null
      
      /* get node type */
      type = child.nodeName.toLowerCase()
      
      /*  objectify attributes */
      attr = SVG.parse.attr(child)
     
      /* create elements */
      switch(type) {
        case 'rect':
        case 'circle':
        case 'ellipse':
          element = context[type](0,0)
        break
        case 'line':
          element = context.line(0,0,0,0)
        break
        case 'text':
          if (child.childNodes.length < 2) {
            element = context[type](child.textContent)
  
          } else {
            var grandchild
  
            element = null
  
            for (j = 0; j < child.childNodes.length; j++) {
              grandchild = child.childNodes[j]
  
              // Otherwise exports with white spaces will introduce fake additional lines...
							/**
							 * If we had an export with whitespaces or if we parse a well-formed SVG, we have to omit
							 * #text items found in the raw sting. Otherwise we'll get unwanted new line tspan items in
							 * the imported SVG graphics.
							 */
							if(grandchild.nodeName.toLowerCase() == "#text")
								continue;

							if (grandchild.nodeName.toLowerCase() == 'tspan') {
								/**
								 * Cut off starting and trailing \n -> otherwise we get additional not need new line chars.
								 * Replace all new lines with space within the tspan, otherwise we'll get unneeded new lines again...
								 * The replacement of inline \n chars with space will not lead to any problems, since in SVG
								 * it should not cause a new line according to specifications.
								 */
								grandchild.textContent = grandchild.textContent.trim().replace(/\n/g," "," ").match(/[\S]+(\s)*/g).join(' ');

								if (element === null)
								/* first time through call the text() function on the current context */
									element = context[type](grandchild.textContent)
  
                else
                  /* for the remaining times create additional tspans */
                  element
                    .tspan(grandchild.textContent)
                    .attr(SVG.parse.attr(grandchild))
              }
            }
  
          }
        break
        case 'path':
          element = context.path(attr['d'])
        break
        case 'polygon':
        case 'polyline':
          element = context[type](attr['points'])
        break
        case 'image':
          element = context.image(attr['xlink:href'])
        break
        case 'g':
        case 'svg':
          element = context[type == 'g' ? 'group' : 'nested']()
          convertNodes(child.childNodes, element, level + 1, store, block)
        break
        case 'defs':
          convertNodes(child.childNodes, context.defs(), level + 1, store, block)
        break
        case 'use':
          element = context.use()
        break
        case 'clippath':
        case 'mask':
          element = context[type == 'mask' ? 'mask' : 'clip']()
          convertNodes(child.childNodes, element, level + 1, store, block)
        break
        case 'lineargradient':
        case 'radialgradient':
          element = context.defs().gradient(type.split('gradient')[0], function(stop) {
            for (var j = 0; j < child.childNodes.length; j++) {
							// Otherwise white spaced export string cannot be read.
							if(child.childNodes[j].nodeName.toLowerCase() == "#text")
								continue;
              stop
                .at({ offset: 0 })
                .attr(SVG.parse.attr(child.childNodes[j]))
                .style(child.childNodes[j].getAttribute('style'))
            }
          })
        break
        case '#comment':
        case '#text':
        case 'metadata':
        case 'desc':
          /* safely ignore these elements */
        break
        case 'marker':
          element = context.defs().marker(
            child.getAttribute('markerWidth') || 0,
            child.getAttribute('markerHeight') || 0
          )
          convertNodes(child.childNodes, element, level + 1, store, block)
          break
        default:
          console.log('SVG Import got unexpected type ' + type, child)
        break
      }
      
      /* parse unexpected attributes */
      switch(type) {
        case 'circle':
          attr.rx = attr.r
          attr.ry = attr.r
          delete attr.r
        break
      }
      
      if (element) {
        /* parse transform attribute */
        transform = SVG.parse.transform(attr.transform)
        delete attr.transform

        /* set attributes and transformations */
        element
          .attr(attr)
          .transform(transform)
  
        /* store element by id */
        if (element.attr('id'))
          store.add(element.attr('id'), element, level == 0)

        /* now that we've set the attributes "rebuild" the text to correctly set the attributes */
        if (type == 'text')
          element.rebuild()
  
        /* call block if given */
        if (typeof block == 'function')
          block.call(element, level)
      }
    }
    
    return context
  }

  SVG.extend(SVG.Container, {
    // Add import method to container elements
    svg: function(raw, block) {
      /* create temporary div to receive svg content */
      var well = document.createElement('div')
        , store = new SVG.ImportStore
      
      /* properly close svg tags and add them to the DOM */
      well.innerHTML = raw
        .replace(/\n/, '')
        .replace(/<([^\s]+)([^<]+?)\/>/g, '<$1$2></$1>')
      
      /* convert nodes to svg elements */
      convertNodes(well.childNodes, this, 0, store, block)
      
      /* mark temporary div for garbage collection */
      well = null
      
      return store
    }
    
  })

  SVG.ImportStore = function() {
    this._importStoreRoots = new SVG.Set
    this._importStore = {}
  }

  SVG.extend(SVG.ImportStore, {
    add: function(key, element, root) {
      /* store element in local store object */
      if (key) {
        if (this._importStore[key]) {
          var oldKey = key
          key += Math.round(Math.random() * 1e16)
          console.warn('Encountered duplicate id "' + oldKey + '". Changed store key to "' + key + '".')
        }

        this._importStore[key] = element
      }

      /* add element to root set */
      if (root === true)
        this._importStoreRoots.add(element)

      return this
    }
    /* get array with root elements */
  , roots: function(iterator) {
      if (typeof iterator == 'function') {
        this._importStoreRoots.each(iterator)

        return this
      } else {
        return this._importStoreRoots.valueOf()
      }
    }
    /* get an element by id */
  , get: function(key) {
      return this._importStore[key]
    }
    /* remove all imported elements */
  , remove: function() {
      return this.roots(function() {
        this.remove()
      })
    }

  })

}).call(this);

///#source 1 1 /Scripts/svg/svg.select.js
/*! svg.select.js - v1.0.0 - 2015-05-27
* https://github.com/Fuzzyma/svg.select.js
* Copyright (c) 2015 Ulrich-Matthias Schäfer; Licensed MIT */
/*jshint -W083*/
;(function (undefined) {

    function SelectHandler(el){

        this.el = el;
        this.parent = el.parent._parent(SVG.Nested) || el._parent(SVG.Doc);
        el.remember('_selectHandler', this);
        this.pointSelection = {isSelected:false};
        this.rectSelection = {isSelected:false};

    }

    SelectHandler.prototype.init = function(value, options){

        var bbox = this.el.bbox();
        this.options = {};

        // Merging the defaults and the options-object together
        for (var i in this.el.select.defaults) {
            this.options[i] = this.el.select.defaults[i];
            if(options[i] !== undefined){
                this.options[i] = options[i];
            }
        }

        this.nested = (this.nested || this.parent.nested()).size(bbox.width, bbox.height).transform(this.el.transform).move(bbox.x, bbox.y);

        // When deepSelect is enabled and the element is a line/polyline/polygon, draw only points for moving
        if(this.options.deepSelect && ['line', 'polyline', 'polygon'].indexOf(this.el.type) !== -1){
            this.selectPoints(value);
        }else{
            this.selectRect(value);
        }

        this.observe();
        this.cleanup();

    };

    SelectHandler.prototype.selectPoints = function(value){

        this.pointSelection.isSelected = value;

        // When set is already there we dont have to create one
        if (this.pointSelection.set){ return this; }

        // Create our set of elements
        this.pointSelection.set = this.parent.set();
        // draw the circles and mark the element as selected
        this.drawCircles();

        return this;

    };

    // create the point-array which contains the 2 points of a line or simply the points-array of polyline/polygon
    SelectHandler.prototype.getPointArray = function(){
        var bbox = this.el.bbox();

        return this.el.type === 'line' ? [
                    [this.el.attr('x1')-bbox.x, this.el.attr('y1')]-bbox.y,
                    [this.el.attr('x2')-bbox.x, this.el.attr('y2')]-bbox.y
                ] : this.el.array.value.map(function(el){ return [el[0]-bbox.x, el[1]-bbox.y]; });
    };

    // The function to draw the circles
    SelectHandler.prototype.drawCircles = function () {

        var _this = this, array = this.getPointArray();

        // go through the array of points
        for (var i = 0, len = array.length; i < len; ++i) {

            // add every point to the set
            this.pointSelection.set.add(

                // a circle with our css-classes and a mousedown-event which fires our event for moving points
                this.nested.circle(this.options.radius)
                    .center(array[i][0], array[i][1])
                    .addClass(this.options.classPoints)
                    .addClass(this.options.classPoints + '_point')
                    .mousedown(
                        (function (k) {
                            return function (ev) {
                                ev = ev || window.event;
                                ev.preventDefault ? ev.preventDefault() : ev.returnValue = false;
                                _this.el.fire('point', {x: ev.pageX, y: ev.pageY, i: k, event:ev});
                            };
                        })(i)
                    )
            );
        }

    };

    // every time a circle is moved, we have to update the positions of our circle
    SelectHandler.prototype.updatePointSelection = function () {
        var array = this.getPointArray();

        this.pointSelection.set.each(function (i) {
            if (this.cx() === array[i][0] && this.cy() === array[i][1]){ return; }
            this.center(array[i][0], array[i][1]);
        });
    };
    
    SelectHandler.prototype.updateRectSelection = function(){
        var bbox = this.el.bbox();
    
        this.rectSelection.set.get(0).attr({
            width: bbox.width,
            height: bbox.height
        });

        // set.get(1) is always in the upper left corner. no need to move it
        if (this.options.points) {
            this.rectSelection.set.get(2).center(bbox.width, 0);
            this.rectSelection.set.get(3).center(bbox.width, bbox.height);
            this.rectSelection.set.get(4).center(0, bbox.height);

            this.rectSelection.set.get(5).center(bbox.width / 2, 0);
            this.rectSelection.set.get(6).center(bbox.width, bbox.height / 2);
            this.rectSelection.set.get(7).center(bbox.width / 2, bbox.height);
            this.rectSelection.set.get(8).center(0, bbox.height / 2);
        }

        if (this.options.rotationPoint) {
            this.rectSelection.set.get(9).center(bbox.width / 2, 20);
        }
    };

    SelectHandler.prototype.selectRect = function(value){

        var _this = this, bbox = this.el.bbox();

        this.rectSelection.isSelected = value;

        // when set is already p
        this.rectSelection.set = this.rectSelection.set || this.parent.set();

        // helperFunction to create a mouse-down function which triggers the event specified in `eventName`
        function getMoseDownFunc(eventName) {
            return function (ev) {
                ev = ev || window.event;
                ev.preventDefault ? ev.preventDefault() : ev.returnValue = false;
                _this.el.fire(eventName, {x: ev.pageX, y: ev.pageY});
            };
        }

        // create the selection-rectangle and add the css-class
        if(!this.rectSelection.set.get(0)){
            this.rectSelection.set.add(this.nested.rect(bbox.width, bbox.height).addClass(this.options.classRect));
        }

        // Draw Points at the edges, if enabled
        if (this.options.points && !this.rectSelection.set.get(1)) {
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(0, 0).attr('class', this.options.classPoints + '_lt').mousedown(getMoseDownFunc('lt')));
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(bbox.width, 0).attr('class', this.options.classPoints + '_rt').mousedown(getMoseDownFunc('rt')));
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(bbox.width, bbox.height).attr('class', this.options.classPoints + '_rb').mousedown(getMoseDownFunc('rb')));
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(0, bbox.height).attr('class', this.options.classPoints + '_lb').mousedown(getMoseDownFunc('lb')));

            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(bbox.width / 2, 0).attr('class', this.options.classPoints + '_t').mousedown(getMoseDownFunc('t')));
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(bbox.width, bbox.height / 2).attr('class', this.options.classPoints + '_r').mousedown(getMoseDownFunc('r')));
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(bbox.width / 2, bbox.height).attr('class', this.options.classPoints + '_b').mousedown(getMoseDownFunc('b')));
            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(0, bbox.height / 2).attr('class', this.options.classPoints + '_l').mousedown(getMoseDownFunc('l')));

            this.rectSelection.set.each(function () {
                this.addClass(_this.options.classPoints);
            });
        }

        // draw rotationPint, if enabled
        if (this.options.rotationPoint && !this.rectSelection.set.get(9)) {

            this.rectSelection.set.add(this.nested.circle(this.options.radius).center(bbox.width / 2, 20).attr('class', this.options.classPoints + '_rot')
                .mousedown(function (ev) {
                    ev = ev || window.event;
                    ev.preventDefault ? ev.preventDefault() : ev.returnValue = false;
                    _this.el.fire('rot', {x: ev.pageX, y: ev.pageY});
                }));

        }

    };

    SelectHandler.prototype.handler = function(){

        var bbox = this.el.bbox();
        this.nested.size(bbox.width, bbox.height).transform(this.el.transform()).move(bbox.x, bbox.y);

        if(this.rectSelection.isSelected){
            this.updateRectSelection();
        }

        if(this.pointSelection.isSelected){
            this.updatePointSelection();
        }

    };

    SelectHandler.prototype.observe = function(){
        var _this = this;

        if(MutationObserver){
            if(this.rectSelection.isSelected || this.pointSelection.isSelected){
                this.observerInst = this.observerInst || new MutationObserver(function(){ _this.handler(); });
                this.observerInst.observe(this.el.node, {attributes: true});
            }else{
                try{
                    this.observerInst.disconnect();
                    delete this.observerInst;
                }catch(e){}
            }
        }else{
            this.el.off('DOMAttrModified.select');

            if(this.rectSelection.isSelected || this.pointSelection.isSelected){
                this.el.on('DOMAttrModified.select', function(){ _this.handler(); } );
            }
        }
    };

    SelectHandler.prototype.cleanup = function(){

        //var _this = this;
    
        if(!this.rectSelection.isSelected && this.rectSelection.set){
            // stop watching the element, remove the selection
            this.rectSelection.set.each(function () {
                this.remove();
            });

            this.rectSelection.set.clear();
            delete this.rectSelection.set;
        }

        if(!this.pointSelection.isSelected && this.pointSelection.set){
            // Remove all points, clear the set, stop watching the element
            this.pointSelection.set.each(function () {
                this.remove();
            });

            this.pointSelection.set.clear();
            delete this.pointSelection.set;
        }

        if(!this.pointSelection.isSelected && !this.rectSelection.isSelected){
            this.nested.remove();
            delete this.nested;
            
            /*try{
                this.observerInst.disconnect();
                delete this.observerInst;
            }catch(e){}
            
            this.el.off('DOMAttrModified.select');
            
        }else{
        
            if(MutationObserver){
                this.observerInst = this.observerInst || new MutationObserver(function(){ _this.handler(); });
                this.observerInst.observe(this.el.node, {attributes: true});
            }else{
                this.el.on('DOMAttrModified.select', function(){ _this.handler(); } )
            }
        */
        }
    };


    SVG.extend(SVG.Element, {
        // Select element with mouse
        select: function (value, options) {

            // Check the parameters and reassign if needed
            if (typeof value === 'object') {
                options = value;
                value = true;
            }

            var selectHandler = this.remember('_selectHandler') || new SelectHandler(this);

            selectHandler.init(value === undefined ? true : value, options || {});

            return this;

        }
    });

    SVG.Element.prototype.select.defaults = {
        points: true,                            // If true, points at the edges are drawn. Needed for resize!
        classRect: 'svg_select_boundingRect',    // Css-class added to the rect
        classPoints: 'svg_select_points',        // Css-class added to the points
        radius: 7,                               // radius of the points
        rotationPoint: true,                     // If true, rotation point is drawn. Needed for rotation!
        deepSelect: false                        // If true, moving of single points is possible (only line, polyline, polyon)
    };

}).call(this);
///#source 1 1 /Scripts/svg/svg.resize.js
/*! svg.resize.js - v1.0.0 - 2015-06-08
* https://github.com/Fuzzyma/svg.resize.js
* Copyright (c) 2015 Ulrich-Matthias Schäfer; Licensed MIT */
;(function () {

    // Calculates the offset of an element
    function offset(el) {
        var x = 0, y = 0;

        if ('doc' in el) {
            var box = el.bbox();
            x = box.x;
            y = box.y;
            el = el.doc().parent;
        }

        while (el.nodeName.toUpperCase() !== 'BODY') {
            x += el.offsetLeft;
            y += el.offsetTop;
            el = el.offsetParent;
        }

        return {x: x, y: y};
    }

    function ResizeHandler(el) {

        el.remember('_resizeHandler', this);

        this.el = el;
        this.parameters = {};
        this.lastUpdateCall = null;

    }

    ResizeHandler.prototype.init = function (options) {

        var _this = this;

        this.stop();

        if (options === 'stop') {
            return;
        }

        this.options = {};

        // Merge options and defaults
        for (var i in this.el.resize.defaults) {
            this.options[i] = this.el.resize.defaults[i];
            if (typeof options[i] !== 'undefined') {
                this.options[i] = options[i];
            }
        }
        
        // We listen to all these events which are specifying different edges
        this.el.on('lt.resize', function(e){ _this.resize(e || window.event); });  // Left-Top
        this.el.on('rt.resize', function(e){ _this.resize(e || window.event); });  // Right-Top
        this.el.on('rb.resize', function(e){ _this.resize(e || window.event); });  // Right-Bottom
        this.el.on('lb.resize', function(e){ _this.resize(e || window.event); });  // Left-Bottom

        this.el.on('t.resize', function(e){ _this.resize(e || window.event); });   // Top
        this.el.on('r.resize', function(e){ _this.resize(e || window.event); });   // Right
        this.el.on('b.resize', function(e){ _this.resize(e || window.event); });   // Bottom
        this.el.on('l.resize', function(e){ _this.resize(e || window.event); });   // Left

        this.el.on('rot.resize', function(e){ _this.resize(e || window.event); }); // Rotation

        this.el.on('point.resize', function(e){ _this.resize(e || window.event); }); // Point-Moving
        
        // This call ensures, that the plugin reacts to a change of snapToGrid immediately
        this.update();
    
    };
    
    ResizeHandler.prototype.stop = function(){
        this.el.off('lt.resize');
        this.el.off('rt.resize');
        this.el.off('rb.resize');
        this.el.off('lb.resize');

        this.el.off('t.resize');
        this.el.off('r.resize');
        this.el.off('b.resize');
        this.el.off('l.resize');

        this.el.off('rot.resize');

        this.el.off('point.resize');

        return this;
    };

    ResizeHandler.prototype.resize = function (event) {

        var _this = this;

        this.parameters = {
            x: event.detail.x,      // x-position of the mouse when resizing started
            y: event.detail.y,      // y-position of the mouse when resizing started
            box: this.el.bbox(),    // The bounding-box of the element
            rbox: this.el.rbox(),   // The "real"-bounding box (transformations included)
            rotation: this.el.transform().rotation  // The current rotation of the element
        };

        // the i-param in the event holds the index of the point which is moved, when using `deepSelect`
        if (event.detail.i !== undefined) {
            // `deepSelect` is possible with lines, too.
            // We have to check that and getting the right point here.
            // So first we build a point-array like the one in polygon and polyline
            var array = this.el.type === 'line' ? [
                [this.el.attr('x1'), this.el.attr('y1')],
                [this.el.attr('x2'), this.el.attr('y2')]
            ] : this.el.array.value;

            // Save the index and the point which is moved
            this.parameters.i = event.detail.i;
            this.parameters.pointCoords = [array[event.detail.i][0], array[event.detail.i][1]];
        }

        // Lets check which edge of the bounding-box was clicked and resize the this.el according to this
        switch (event.type) {

            // Left-Top-Edge
            case 'lt':
                // We build a calculating function for every case which gives us the new position of the this.el
                this.calc = function (diffX, diffY) {
                    // The procedure is always the same
                    // First we snap the edge to the given grid (snapping to 1px grid is normal resizing)
                    var snap = this.snapToGrid(diffX, diffY);

                    // Now we check if the new height and width still valid (> 0)
                    if (this.parameters.box.width - snap[0] > 0 && this.parameters.box.height - snap[1] > 0) {
                        // ...if valid, we resize the this.el (which can include moving because the coord-system starts at the left-top and this edge is moving sometimes when resized)
                        this.el.move(this.parameters.box.x + snap[0], this.parameters.box.y + snap[1]).size(this.parameters.box.width - snap[0], this.parameters.box.height - snap[1]);
                    }
                };
                break;

            // Right-Top
            case 'rt':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 1 << 1);
                    if (this.parameters.box.width + snap[0] > 0 && this.parameters.box.height - snap[1] > 0) {
                        this.el.move(this.parameters.box.x, this.parameters.box.y + snap[1]).size(this.parameters.box.width + snap[0], this.parameters.box.height - snap[1]);
                    }
                };
                break;

            // Right-Bottom
            case 'rb':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 0);
                    if (this.parameters.box.width + snap[0] > 0 && this.parameters.box.height + snap[1] > 0) {
                        this.el.move(this.parameters.box.x, this.parameters.box.y).size(this.parameters.box.width + snap[0], this.parameters.box.height + snap[1]);
                    }
                };
                break;

            // Left-Bottom
            case 'lb':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 1);
                    if (this.parameters.box.width - snap[0] > 0 && this.parameters.box.height + snap[1] > 0) {
                        this.el.move(this.parameters.box.x + snap[0], this.parameters.box.y).size(this.parameters.box.width - snap[0], this.parameters.box.height + snap[1]);
                    }
                };
                break;

            // Top
            case 't':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 1 << 1);
                    if (this.parameters.box.height - snap[1] > 0) {
                        this.el.move(this.parameters.box.x, this.parameters.box.y + snap[1]).height(this.parameters.box.height - snap[1]);
                    }
                };
                break;

            // Right
            case 'r':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 0);
                    if (this.parameters.box.width + snap[0] > 0) {
                        this.el.move(this.parameters.box.x, this.parameters.box.y).width(this.parameters.box.width + snap[0]);
                    }
                };
                break;

            // Bottom
            case 'b':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 0);
                    if (this.parameters.box.height + snap[1] > 0) {
                        this.el.move(this.parameters.box.x, this.parameters.box.y).height(this.parameters.box.height + snap[1]);
                    }
                };
                break;

            // Left
            case 'l':
                // s.a.
                this.calc = function (diffX, diffY) {
                    var snap = this.snapToGrid(diffX, diffY, 1);
                    if (this.parameters.box.width - snap[0] > 0) {
                        this.el.move(this.parameters.box.x + snap[0], this.parameters.box.y).width(this.parameters.box.width - snap[0]);
                    }
                };
                break;

            // Rotation
            case 'rot':
                // s.a.
                this.calc = function (diffX, diffY) {

                    // yes this is kinda stupid but we need the mouse coords back...
                    var current = {x: diffX + this.parameters.x, y: diffY + this.parameters.y};
                    
                    // we need the offset of the element to calculate our angle
                    var off = offset(this.el);

                    // start minus middle
                    var sAngle = Math.atan2((this.parameters.y - off.y - this.parameters.rbox.height / 2), (this.parameters.x - off.x - this.parameters.rbox.width / 2));

                    // end minus middle
                    var pAngle = Math.atan2((current.y - off.y - this.parameters.rbox.height / 2), (current.x - off.x - this.parameters.rbox.width / 2));

                    var angle = (pAngle - sAngle) * 180 / Math.PI;

                    // We have to move the element to the center of the rbox first and change the rotation afterwards
                    // because rotation always works around a rotation-center, which is changed when moving the this.el.
                    // We also set the new rotation center to the center of the rbox.
                    // The -0.5 and -1 is tuning since the box is jumping for a few px when starting the rotation.
                    this.el.center(this.parameters.rbox.cx - 0.5, this.parameters.rbox.cy - 1).transform({rotation: this.parameters.rotation + angle - angle % this.options.snapToAngle, cx: this.parameters.rbox.cx, cy: this.parameters.rbox.cy});
                };
                break;

            // Moving one single Point (needed when an this.el is deepSelected which means you can move every single point of the object)
            case 'point':
                this.calc = function (diffX, diffY) {

                    // Snapping the point to the grid
                    var snap = this.snapToGrid(diffX, diffY, this.parameters.pointCoords[0], this.parameters.pointCoords[1]);

                    // We build an object to handle the different properties of a line, if needed
                    var array = this.el.type === 'line' ? [
                        {
                            x1: this.parameters.pointCoords[0] + snap[0],
                            y1: this.parameters.pointCoords[1] + snap[1]
                        },
                        {
                            x2: this.parameters.pointCoords[0] + snap[0],
                            y2: this.parameters.pointCoords[1] + snap[1]
                        }
                    ] : this.el.array.value;    // Otherwise we need the normal point-array


                    if (this.el.type === 'line') {
                        // Now we can specify the correct property using the array and set them
                        this.el.attr(array[this.parameters.i]);
                        return;
                    }

                    // Changing the moved point in the array
                    array[this.parameters.i][0] = this.parameters.pointCoords[0] + snap[0];
                    array[this.parameters.i][1] = this.parameters.pointCoords[1] + snap[1];

                    // And plot the new this.el
                    this.el.plot(array);
                };
        }

        // When resizing started, we have to register events for...
        SVG.on(window, 'mousemove.resize', function (e) {
            _this.update(e || window.event);
        });    // mousemove to keep track of the changes and...
        SVG.on(window, 'mouseup.resize', function () {
            _this.done();
        });        // mouseup to know when resizing stops

    };

    // The update-function redraws the element every time the mouse is moving
    ResizeHandler.prototype.update = function (event) {

        if (!event) {
            if (this.lastUpdateCall) {
                this.calc(this.lastUpdateCall[0], this.lastUpdateCall[1]);
            }
            return;
        }

        // Calculate the difference between the mouseposition at start and now
        var diffX = event.pageX - this.parameters.x
            , diffY = event.pageY - this.parameters.y;

        this.lastUpdateCall = [diffX, diffY];

        // Calculate the new position and height / width of the element
        this.calc(diffX, diffY);
    };

    // Is called on mouseup. 
    // Removes the update-function from the mousemove event
    ResizeHandler.prototype.done = function () {
        this.lastUpdateCall = null;
        SVG.off(window, 'mousemove.resize');
        SVG.off(window, 'mouseup.resize');
        this.el.fire('resizedone');
    };

    // The flag is used to determine whether the resizing is used with a left-Point (first bit) and top-point (second bit)
    // In this cases the temp-values are calculated differently
    ResizeHandler.prototype.snapToGrid = function (diffX, diffY, flag, pointCoordsY) {

        var temp;

        // If `pointCoordsY` is given, a single Point has to be snapped (deepSelect). That's why we need a different temp-value
        if (pointCoordsY) {
            // Note that flag = pointCoordsX in this case
            temp = [(flag + diffX) % this.options.snapToGrid, (pointCoordsY + diffY) % this.options.snapToGrid];
        } else {
            // We check if the flag is set and if not we set a default-value (both bits set - which means upper-left-edge)
            flag = flag == null ? 1 | 1 << 1 : flag;
            temp = [(this.parameters.box.x + diffX + (flag & 1 ? 0 : this.parameters.box.width)) % this.options.snapToGrid, (this.parameters.box.y + diffY + (flag & (1 << 1) ? 0 : this.parameters.box.height)) % this.options.snapToGrid];
        }

        diffX -= (Math.abs(temp[0]) < this.options.snapToGrid / 2 ? temp[0] : temp[0] - this.options.snapToGrid) + (temp[0] < 0 ? this.options.snapToGrid : 0);
        diffY -= (Math.abs(temp[1]) < this.options.snapToGrid / 2 ? temp[1] : temp[1] - this.options.snapToGrid) + (temp[1] < 0 ? this.options.snapToGrid : 0);
        return [diffX, diffY];

    };

    SVG.extend(SVG.Element, {
        // Resize element with mouse
        resize: function (options) {

            (this.remember('_resizeHandler') || new ResizeHandler(this)).init(options || {});

            return this;

        }

    });

    SVG.Element.prototype.resize.defaults = {
        snapToAngle: 0.1,    // Specifies the speed the rotation is happening when moving the mouse
        snapToGrid: 1        // Snaps to a grid of `snapToGrid` Pixels
    };

}).call(this);
///#source 1 1 /Scripts/svg/svg.shapes.js
// svg.shapes.js 0.11 - Copyright (c) 2013 Wout Fierens - Licensed under the MIT license

;(function() {

  var defaults = {
      spikes: 7
    , inner:  50
    , outer:  100
    , edges:  7
    , radius: 100
    }
  
  // Add builders to polygon
  SVG.extend(SVG.Polyline, SVG.Polygon, {
    // Dynamic star shape
    star: function (settings) {
      var box = this.bbox()

      /* merge user input */
      this.settings = merge(this.settings, settings)
      
      return this.plot(SVG.shapes.star(this.settings).move(box.x, box.y))
    }
    // Dynamic ngon shape
  , ngon: function(settings) {
      var box = this.bbox()

      /* merge user input */
      this.settings = merge(this.settings, settings)

      return this.plot(SVG.shapes.ngon(this.settings).move(box.x, box.y))
    }

  })

  // Make shapes animatable
  SVG.extend(SVG.FX, {
    // Animatable star
    star: function(settings) {
      var box = this.bbox()

      /* merge user input */
      this.target.settings = merge(this.target.settings, settings)

      return this.plot(SVG.shapes.star(this.target.settings).move(box.x, box.y))
    }
  , // Animatable ngon
    ngon: function(settings) {
      var box = this.bbox()

      /* merge user input */
      this.target.settings = merge(this.target.settings, settings)

      return this.plot(SVG.shapes.ngon(this.target.settings).move(box.x, box.y))
    }

  })


  // Shape generator
  SVG.shapes = {
    // Star generator
    star: function(settings) {
      var i, a, x, y
        , points  = []
        , spikes  = typeof settings.spikes == 'number' ? settings.spikes : defaults.spikes
        , inner   = typeof settings.inner  == 'number' ? settings.inner  : defaults.inner 
        , outer   = typeof settings.outer  == 'number' ? settings.outer  : defaults.outer 
        , degrees = 360 / spikes

      for (i = 0; i < spikes; i++) {
        a = i * degrees + 90
        x = outer + inner * Math.cos(a * Math.PI / 180)
        y = outer + inner * Math.sin(a * Math.PI / 180)

        points.push([x, y])

        a += degrees / 2
        x = outer + outer * Math.cos(a * Math.PI / 180)
        y = outer + outer * Math.sin(a * Math.PI / 180)

        points.push([x, y])
      }

      return new SVG.PointArray(points)
    }
    // Ngon generator
  , ngon: function(settings) {
      var i, a, x, y
        , points  = []
        , edges   = typeof settings.edges  == 'number' ? settings.edges  : defaults.edges 
        , radius  = typeof settings.radius == 'number' ? settings.radius : defaults.radius
        , degrees = 360 / edges
  
      for (i = 0; i < edges; i++) {
        a = i * degrees - 90
        x = radius + radius * Math.cos(a * Math.PI / 180)
        y = radius + radius * Math.sin(a * Math.PI / 180)
  
        points.push([x, y])
      }
  
      return new SVG.PointArray(points)
    }
  }

  // Helpers
  function merge(target, object) {
    var key
      , settings = {}

    /* ensure objects */
    target = target || {}
    object = object || {}

    /* merge object */
    for (key in defaults)
      settings[key] = typeof object[key] === 'number' ?
        object[key] :
      typeof target[key] === 'number' ?
        target[key] :
        defaults[key]

    return settings
  }
  
}).call(this)
///#source 1 1 /Scripts/svg/svg.topath.js
// svg.topath.js 0.4 - Copyright (c) 2014 Wout Fierens - Licensed under the MIT license
;(function() {

	SVG.extend(SVG.Shape, {
		// Convert element to path
		toPath: function(replace) {
			var	w, h, rx, ry, d, path
				, trans = this.trans
				, box = this.bbox()
				, x = 0
				, y = 0
			
			switch(this.type) {
				case 'rect':
					w  = this.attr('width')
					h  = this.attr('height')
					rx = this.attr('rx')
					ry = this.attr('ry')

					// normalise radius values, just like the original does it (or should do)
					if (rx < 0) rx = 0
					if (ry < 0) ry = 0
					rx = rx || ry
					ry = ry || rx
					if (rx > w / 2) rx = w / 2
					if (ry > h / 2) ry = h / 2
					
					if (rx && ry) {
						// if there are round corners
						d = [
							'M' + rx + ' ' + y
						, 'H' + (w - rx)
						, 'A' + rx + ' ' + ry + ' 0 0 1 ' + w + ' ' + ry
						, 'V' + (h - ry)
						, 'A' + rx + ' ' + ry + ' 0 0 1 ' + (w - rx) + ' ' + h
						, 'H' + rx
						, 'A' + rx + ' ' + ry + ' 0 0 1 ' + x + ' ' + (h - ry)
						, 'V' + ry
						, 'A' + rx + ' ' + ry + ' 0 0 1 ' + rx + ' ' + y
						, 'z'
						]
					} else {
						// no round corners, no need to draw arcs
						d = [
							'M' + x + ' ' + y
						, 'H' + w
						, 'V' + h
						, 'H' + x
						, 'V' + y
						, 'z'
						]
					}

					x = this.attr('x')
					y = this.attr('y')
					
				break
				case 'circle':
				case 'ellipse':
					rx = this.type == 'ellipse' ? this.attr('rx') : this.attr('r')
					ry = this.type == 'ellipse' ? this.attr('ry') : this.attr('r')

					d = [
						'M' + rx + ' ' + y
					, 'A' + rx + ' ' + ry + ' 0 0 1 ' + (rx * 2) + ' ' + ry
					, 'A' + rx + ' ' + ry + ' 0 0 1 ' + rx 			 + ' ' + (ry * 2)
					, 'A' + rx + ' ' + ry + ' 0 0 1 ' + x 			 + ' ' + ry
					, 'A' + rx + ' ' + ry + ' 0 0 1 ' + rx 			 + ' ' + y
					, 'z'
					]

					x = this.attr('cx') - rx
					y = this.attr('cy') - ry
				break
				case 'polygon':
				case 'polyline':
					this.move(0,0)

					d = []

					for (var i = 0; i < this.array.value.length; i++)
						d.push((i == 0 ? 'M' : 'L') + this.array.value[i][0] + ' ' + this.array.value[i][1])

					if (this.type == 'polygon')
						d.push('Z')

					this.move(box.x, box.y)

					x = box.x
					y = box.y
				break
				case 'line':
					this.move(0,0)

					d = [
						'M' + this.attr('x1') + ' ' + this.attr('y1')
					, 'L' + this.attr('x2') + ' ' + this.attr('y2')
					]

					this.move(box.x, box.y)

					x = box.x
					y = box.y
				break
				case 'path':
					path = this.clone()
					path.unbiased = true
					path.plot(this.attr('d'))

					x = box.x
					y = box.y
				break
				default: 
					console.log('SVG toPath got unsupported type ' + this.type, this)
				break
			}

			if (Array.isArray(d)) {
				// create path element
				path = this.parent
					.path(d.join(''), true)
					.move(x + trans.x, y + trans.y)
					.attr(normaliseAttributes(this.attr()))

				// insert interpreted path after original
				this.after(path)
			}
			
			if (this instanceof SVG.Shape && path) {
				// store original details in data attributes
				path
					.data('topath-type', this.type)
					.data('topath-id', this.attr('id'))

				// remove original if required
				if (replace === true)
					this.remove()
				else
					path.original = this
			}

			return path
		}

	})

	SVG.extend(SVG.Parent, {
		// Recruisive path conversion
		toPath: function(replace) {
			// cloning children array so that we don't touch the paths we create
      var children = [].slice.call(this.children())

      // convert top paths
      for (var i = children.length - 1; i >= 0; i--)
        if (typeof children[i].toPath === 'function')
          children[i].toPath(sample, replace)
      
      return this
		}
	})

	// Normalise attributes
	function normaliseAttributes(attr) {
		for (var a in attr)
			if (!/fill|stroke|opacity/.test(a))
				delete attr[a]

		return attr
	}

}).call(this);

