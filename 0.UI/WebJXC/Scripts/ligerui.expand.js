/*
默认参数 扩展
*/

$.extend($.ligerDefaults.Grid, {
    rownumbers: true,
    pageSize: 20,
    headerRowHeight: 30,
    onError: function (result, b) {
        LG.tip('发现系统错误 ' + b);
    }
});

$.extend($.ligerDefaults.Tab, {
    contextmenu: false
});

$.extend($.ligerDefaults.ComboBox, {
    slide: false
});

/*
表格 扩展
*/

$.extend($.ligerui.controls.Grid.prototype, {
    addEditRow: function (rowdata) {
        var g = this;
        rowdata = g.add(rowdata);
        return g.beginEdit(rowdata);
    },
    getEditingRow: function () {
        var g = this;
        for (var i = 0, l = g.rows.length; i < l; i++) {
            if (g.rows[i]._editing) return g.rows[i];
        }
        return null;
    },
    getChangedRows: function () {
        var g = this, changedRows = [];
        pushRows(g.getDeleted(), 'delete');
        pushRows(g.getUpdated(), 'update');
        pushRows(g.getAdded(), 'add');
        return changedRows;

        function pushRows(rows, status) {
            if (!rows || !rows instanceof Array) return;
            for (var i = 0, l = rows.length; i < l; i++) {
                changedRows.push($.extend({}, rows[i], { __status: status }));
            }
        }

    }
});

/*
表格格式化函数扩展
*/



//扩展 percent 百分比 类型的格式化函数(0到1之间)
$.ligerDefaults.Grid.formatters['percent'] = function (value, column) {
    if (value < -1) value = value / 100;
    if (value > 1) value = value / 100;
    var precision = column.editor ? (column.editor.precision || 0) : 2;
    return "<div style='text-align:right;color:" + (value >= 0 ? "red" : "green") + "'>" + (value * 100).toFixed(precision) + "%</div>";
};

//扩展 numberbox 类型的格式化函数
$.ligerDefaults.Grid.formatters['number'] = function (value, column) {
    if (!value) return 0.00;
    var precision = column.editor ? (column.editor.precision || 2) : 2;
    return value.toFixed(precision);
};
//扩展currency类型的格式化函数
$.ligerDefaults.Grid.formatters['currency'] = function (num, column) {
    //num 当前的值
    //column 列信息
    if (!num) return "￥0.00";
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "￥0.00";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
        num.substring(num.length - (4 * i + 3));
    return "￥" + (((sign) ? '' : '-') + '' + num + '.' + cents);
};

//扩展 checkbox 类型的格式化函数
$.ligerDefaults.Grid.formatters['checkbox'] = function (value, column) {
    var iconHtml = '<div class="chk-icon';
    if (value) iconHtml += " chk-icon-selected";
    iconHtml += '"';
    iconHtml += ' gridid = "' + this.id + '"';
    iconHtml += ' columnname = "' + column.name + '"';
    iconHtml += '></div>';
    return iconHtml;
};

//扩展 icon 类型的格式化函数
$.ligerDefaults.Grid.formatters['icon'] = function (value, column) {
    return "<img src='"+value+"'></img>";
};
/*
表格编辑器
*/

//扩展一个 百分比输入框 的编辑器(0到1之间)
$.ligerDefaults.Grid.editors['percent'] = {
    create: function (container, editParm) {
        var column = editParm.column;
        var precision = column.editor.precision || 0;
        var input = $("<input type='text' style='text-align:right' class='l-text' />");
        input.bind('keypress', function (e) {
            var keyCode = window.event ? e.keyCode : e.which;
            return keyCode >= 48 && keyCode <= 57 || keyCode == 46 || keyCode == 8;
        });
        input.bind('blur', function () {
            var showVal = input.val();
            showVal.replace('%', '');
            input.val(parseFloat(showVal).toFixed(precision));
        });
        container.append(input);
        return input;
    },
    getValue: function (input, editParm) {
        var showVal = input.val();
        showVal.replace('%', '');
        var value = parseFloat(showVal) * 0.01;
        if (value < 0) value = 0;
        if (value > 1) value = 1;
        return value;
    },
    setValue: function (input, value, editParm) {
        var column = editParm.column;
        var precision = column.editor.precision || 0;
        if (value < 0) value = 0;
        if (value > 1) value = 1;
        var showVal = (value * 100).toFixed(precision) + "%";
        input.val(showVal);
    },
    resize: function (input, width, height, editParm) {
        input.width(width).height(height);
    }
};

//扩展一个 数字输入 的编辑器
$.ligerDefaults.Grid.editors['number'] = {
    create: function (container, editParm) {
        var column = editParm.column;
        var precision = column.editor.precision;
        var input = $("<input type='text' style='text-align:right' class='l-text' />");
        input.bind('keypress', function (e) {
            var keyCode = window.event ? e.keyCode : e.which;
            return keyCode >= 48 && keyCode <= 57 || keyCode == 46 || keyCode == 8;
        });
        input.bind('blur', function () {
            var value = input.val();
            input.val(parseFloat(value).toFixed(precision));
        });
        container.append(input);
        return input;
    },
    getValue: function (input, editParm) {
        return parseFloat(input.val());
    },
    setValue: function (input, value, editParm) {
        var column = editParm.column;
        var precision = column.editor.precision;
        if (!value) value = 0.00;
        input.val(value.toFixed(precision));
    },
    resize: function (input, width, height, editParm) {
        input.width(width).height(height);
    }
};

$.ligerDefaults.Grid.editors['int'] =
     $.ligerDefaults.Grid.editors['float'] =
     $.ligerDefaults.Grid.editors['spinner'] =
     {
         create: function (container, editParm) {
             var column = editParm.column;
             var input = $("<input type='text'/>");
             container.append(input);
             input.css({ border: '#6E90BE' })
             var options = {
                 type: column.editor.type == 'float' ? 'float' : 'int'
             };
             $.extend(options, column.editor);
             if (column.editor.minValue != undefined) options.minValue = column.editor.minValue;
             if (column.editor.maxValue != undefined) options.maxValue = column.editor.maxValue;
             
             input.ligerSpinner(options);
             return input;
         },
         getValue: function (input, editParm) {
             var column = editParm.column;
             var isInt = column.editor.type == "int";
             if (isInt)
                 return parseInt(input.val(), 10);
             else
                 return parseFloat(input.val());
         },
         setValue: function (input, value, editParm) {
             input.val(value);
         },
         resize: function (input, width, height, editParm) {
             input.liger('option', 'width', width);
             input.liger('option', 'height', height);
         },
         destroy: function (input, editParm) {
             input.liger('destroy');
         }
     };



//扩展 ligerGrid 的 搜索功能(高级自定义查询).应用: demos/filter/grid.htm
$.ligerui.controls.Grid.prototype.showFilter = function () {
    var g = this, p = this.options;
    if (g.winfilter) {
        g.winfilter.show();
        return;
    }
    var filtercontainer = $('<div id="' + g.id + '_filtercontainer"></div>').width(380).height(120).hide();
    var filter = filtercontainer.ligerFilter({ fields: getFields() });
    return g.winfilter = $.ligerDialog.open({
        width: 420, height: 208,
        target: filtercontainer, isResize: true, top: 50,
        buttons: [
            { text: '确定', onclick: function (item, dialog) { loadData(g, filter.getData()); dialog.hide(); } },
            { text: '取消', onclick: function (item, dialog) { dialog.hide(); } }
            ]
    });

    //将grid的columns转换为filter的fields
    function getFields() {
        var fields = [];
        //如果是多表头，那么g.columns为最低级的列
        $(g.columns).each(function () {
            var o = { name: this.name, display: this.display };
            var isNumber = this.type == "int" || this.type == "number" || this.type == "float";
            var isDate = this.type == "date";
            if (isNumber) o.type = "number";
            if (isDate) o.type = "date";
            if (this.editor) {
                o.editor = this.editor;
            }
            fields.push(o);
        });
        return fields;
    }

};


function loadData(g, data) {
    //    if (g.options.dataAction == "server") {
    //服务器过滤数据
    loadServerData(g, data);
    //    }
    //    else {
    //        //本地过滤数据
    //        loadClientData(g, data);
    //    }
}

function loadServerData(g, data) {
    var param = [];
    if (data && data.rules && data.rules.length) {
        $.each(data.rules, function () {
            param.push({ name: this.field.replace("_search", ""), value: this.value });
        });
    }
    g.loadServerData(param);
//    LG.ajax({
//        url: g.options.url,
//        data: param,
//        success: function (d) {
//            g.loadData({ Rows: d });
//            g.changePage("first");
//        },
//        error: function () {
//            LG.tip('加载失败');
//        }
//    });

}
function loadClientData(g, data) {
    var fnbody = ' return  ' + filterTranslator.translateGroup(data);
    g.options.data = $.extend(true, {}, g.originData);
    g.loadData(new Function("o", fnbody));
}

var filterTranslator = {

    translateGroup: function (group) {
        var out = [];
        if (group == null) return " 1==1 ";
        var appended = false;
        out.push('(');
        if (group.rules != null) {
            for (var i in group.rules) {
                var rule = group.rules[i];
                if (appended)
                    out.push(this.getOperatorQueryText(group.op));
                out.push(this.translateRule(rule));
                appended = true;
            }
        }
        if (group.groups != null) {
            for (var j in group.groups) {
                var subgroup = group.groups[j];
                if (appended)
                    out.push(this.getOperatorQueryText(group.op));
                out.push(this.translateGroup(subgroup));
                appended = true;
            }
        }
        out.push(')');
        if (appended == false) return " 1==1 ";
        return out.join('');
    },

    translateRule: function (rule) {
        var out = [];
        if (rule == null) return " 1==1 ";
        if (rule.op == "like" || rule.op == "startwith" || rule.op == "endwith") {
            out.push('/');
            if (rule.op == "startwith")
                out.push('^');
            out.push(rule.value);
            if (rule.op == "endwith")
                out.push('$');
            out.push('/i.test(');
            out.push('o["');
            out.push(rule.field);
            out.push('"]');
            out.push(')');
            return out.join('');
        }
        out.push('o["');
        out.push(rule.field);
        out.push('"]');
        out.push(this.getOperatorQueryText(rule.op));
        out.push('"');
        out.push(rule.value);
        out.push('"');
        return out.join('');
    },


    getOperatorQueryText: function (op) {
        switch (op) {
            case "equal":
                return " == ";
            case "notequal":
                return " != ";
            case "greater":
                return " > ";
            case "greaterorequal":
                return " >= ";
            case "less":
                return " < ";
            case "lessorequal":
                return " <= ";
            case "and":
                return " && ";
            case "or":
                return " || ";
            default:
                return " == ";
        }
    }

};

$.extend($.ligerui.controls.Grid.prototype, {
    loadServerData: function (param, clause) {
        var g = this, p = this.options;
        var ajaxOptions = {
            type: p.method,
            url: p.url,
            data: param,
            async: p.async,
            dataType: 'json',
            beforeSend: function () {
                if (g.hasBind('loading')) {
                    g.trigger('loading');
                }
                else {
                    g.toggleLoading(true);
                }
            },
            success: function (data) {
                g.trigger('success', [data, g]);
                if (!data || !data[p.root] || !data[p.root].length) {
                    if (g.options.tree) {
                        var t = g.options.tree;
                        data = arrayToTree(data, t.id, t.pid);
                    }
                    g.loadData({ Rows: data });
                    return;
                }
                g.data = data;
                if (p.dataAction == "server") {
                    g.currentData = g.data;
                }
                else {
                    g.filteredData = g.data;
                    if (clause) g.filteredData[p.root] = g._searchData(g.filteredData[p.root], clause);
                    if (p.usePager)
                        g.currentData = g._getCurrentPageData(g.filteredData);
                    else
                        g.currentData = g.filteredData;
                }
                g._showData.ligerDefer(g, 10, [g.currentData]);
            },
            complete: function () {
                g.trigger('complete', [g]);
                if (g.hasBind('loaded')) {
                    g.trigger('loaded', [g]);
                }
                else {
                    g.toggleLoading.ligerDefer(g, 10, [false]);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                g.currentData = g.data = {};
                g.currentData[p.root] = g.data[p.root] = [];
                g.currentData[p.record] = g.data[p.record] = 0;
                g.toggleLoading.ligerDefer(g, 10, [false]);
                $(".l-bar-btnload span", g.toolbar).removeClass("l-disabled");
                g.trigger('error', [XMLHttpRequest, textStatus, errorThrown]);
            }
        };
        if (p.contentType) ajaxOptions.contentType = p.contentType;
        $.ajax(ajaxOptions);
    }
});


/*
表单 扩展
*/


$.extend($.ligerui.controls.ComboBox.prototype, {
    _setHeight: function (value) {
        var g = this;
        if (value > 10) {
            g.wrapper.height(value);
            g.inputText.height(value);
            g.link.height(value);
            g.textwrapper.css({ width: value });
        }
    }
});



//扩展 DateEditor 的updateStyle方法
$.ligerui.controls.DateEditor.prototype.updateStyle = function () {
    var g = this, p = this.options;
    //Grid的date默认格式化函数就有对日期的处理
    var v = $.ligerDefaults.Grid.formatters['date'](g.inputText.val(), { format: p.format });
    g.inputText.val(v);
}


/*
下拉框 combobox
*/

//下拉框 加载文本值(有的时候在数据库只是返回了id值，并没有加载文本值，需要调用这个方法，远程获取)
$.ligerui.controls.ComboBox.prototype.loadText = function (options) {
    var g = this, p = this.options;
    options = $.extend({
        url: '/config/select',
        id: null,
        idfield: null,
        textfield: null
    }, options || {});
    var value = options.value || g.getValue();

    $.ajax({
        cache: false,
        async: true,
        dataType: 'json', type: 'post',
        url: options.url,
        data: {
            id: options.id
        },
        success: function (data) {
            if (!data || !data.length) return;
            $.each(data, function (i,n) {
                if (data[i]['id'] == value) {
                    g._changeValue(data[i]['id'], data[i]['text']);
                    return;
                }
            });
            
        }
    });
};


function arrayToTree(data, id, pid)      //将ID、ParentID这种数据格式转换为树格式
{
    if (!data || !data.length) return [];
    var targetData = [];                    //存储数据的容器(返回) 
    var records = {};
    var itemLength = data.length;           //数据集合的个数
    for (var i = 0; i < itemLength; i++) {
        var o = data[i];
        records[o[id]] = o;
    }
    for (var i = 0; i < itemLength; i++) {
        var currentData = data[i];
        var parentData = records[currentData[pid]];
        if (!parentData) {
            targetData.push(currentData);
            continue;
        }
        parentData.children = parentData.children || [];
        parentData.children.push(currentData);
    }
    return targetData;
}
