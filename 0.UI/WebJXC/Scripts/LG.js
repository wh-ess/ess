/// <reference path="jquery-1.8.2.min.js" />

(function ($) {

    $.ajaxSetup({
        cache: false
    });
    // 全局系统对象
    window['LG'] = {};

    //input[text]单击选择
    $("input[type=text]").on("click", function () {
        this.select();
    });

    //屏蔽回车
    function keypress(e) {
        var currKey = 0, e = e || event;
        if (e.keyCode == 13) {
            e.keyCode = 0;
            return false;
        }
    }
    document.onkeypress = keypress;

    LG.cookies = (function () {
        var fn = function () {
        };
        fn.prototype.get = function (name) {
            var cookieValue = "";
            var search = name + "=";
            if (document.cookie.length > 0) {
                offset = document.cookie.indexOf(search);
                if (offset != -1) {
                    offset += search.length;
                    end = document.cookie.indexOf(";", offset);
                    if (end == -1) end = document.cookie.length;
                    cookieValue = decodeURIComponent(document.cookie.substring(offset, end))
                }
            }
            return cookieValue;
        };
        fn.prototype.set = function (cookieName, cookieValue, DayValue) {
            var expire = "";
            var day_value = 1;
            if (DayValue != null) {
                day_value = DayValue;
            }
            expire = new Date((new Date()).getTime() + day_value * 86400000);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + encodeURIComponent(cookieValue) + ";path=/" + expire;
        }
        fn.prototype.remvoe = function (cookieName) {
            var expire = "";
            expire = new Date((new Date()).getTime() - 1);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + escape("") + ";path=/" + expire;
            /*path=/*/
        };

        return new fn();
    })();

    // 右下角的提示框
    LG.tip = function (message) {
        if (LG.wintip) {
            LG.wintip.set('content', message);
            LG.wintip.show();
        }
        else {
            LG.wintip = $.ligerDialog.tip({ content: message });
        }
        setTimeout(function () {
            LG.wintip.hide()
        }, 4000);
    };

    // 预加载图片
    LG.prevLoadImage = function (rootpath, paths) {
        for (var i in paths) {
            $('<img />').attr('src', rootpath + paths[i]);
        }
    };

    // 显示loading
    LG.showLoading = function (message) {
        message = message || "正在加载中...";
        $('body').append("<div class='jloading'>" + message + "</div>");
        $.ligerui.win.mask();
    };

    // 隐藏loading
    LG.hideLoading = function (message) {
        $('body > div.jloading').remove();
        $.ligerui.win.unmask({ id: new Date().getTime() });
    }

    // 显示成功提示窗口
    LG.showSuccess = function (message, callback) {
        if (typeof (message) == "function" || arguments.length == 0) {
            callback = message;
            message = "操作成功!";
        }
        $.ligerDialog.success(message, '提示信息', callback);
    };

    // 显示失败提示窗口
    LG.showError = function (message, callback) {
        if (typeof (message) == "function" || arguments.length == 0) {
            callback = message;
            message = "操作失败!";
        }
        $.ligerDialog.error(message, '提示信息', callback);
    };

    // 预加载dialog的图片
    LG.prevDialogImage = function (rootPath) {
        rootPath = rootPath || "";
        LG.prevLoadImage(rootPath + 'lib/ligerUI/skins/Aqua/images/win/', ['dialog-icons.gif']);
        LG.prevLoadImage(rootPath + 'lib/ligerUI/skins/Gray/images/win/', ['dialogicon.gif']);
    };

    // 提交服务器请求
    //返回json格式
    //1,提交给类 options.type  方法 options.method 处理
    //2,并返回 AjaxResult(这也是一个类)类型的的序列化好的字符串
    LG.ajax = function (options) {
        var p = options || {};
        $.ajax({
            cache: false,
            url: p.url,
            data: p.data,
            dataType: p.dataType ? p.dataType : 'json',
            type: p.type ? p.type : 'post',
            beforeSend: function () {
                LG.loading = true;
                if (p.beforeSend)
                    p.beforeSend();
                else
                    LG.showLoading(p.loading);
            },
            complete: function () {
                LG.loading = false;
                if (p.complete)
                    p.complete();
                else
                    LG.hideLoading();
            },
            success: function (result) {
                if (p.success)
                    p.success(result);
            },
            error: function (result, b) {
                if (p.error)
                    p.error(result);
                else
                    LG.tip(result.responseText.slice(result.responseText.indexOf("<title>") + 7, result.responseText.indexOf("</title>")));
            }
        });
    };

    // 获取当前页面的MenuNo
    //优先级1：如果页面存在MenuNo的表单元素，那么加载它的值
    //优先级2：加载QueryString，名字为MenuNo的值
    LG.getPageMenuNo = function () {
        var menuno = $("#MenuNo").val();
        if (!menuno) {
            menuno = getQueryStringByName("MenuNo");
        }
        return menuno;
    };

    // 创建按钮
    LG.createButton = function (options) {
        var p = $.extend({
            appendTo: $('body')
        }, options || {});
        var btn = $('<div class="button button2 buttonnoicon" style="width:60px"><div class="button-l"> </div><div class="button-r"> </div> <span></span></div>');
        if (p.icon) {
            btn.removeClass("buttonnoicon");
            btn.append('<div class="button-icon"> <img src="../' + p.icon + '" /> </div> ');
        }
        //绿色皮肤
        if (p.green) {
            btn.removeClass("button2");
        }
        if (p.width) {
            btn.width(p.width);
        }
        if (p.click) {
            btn.click(p.click);
        }
        if (p.text) {
            $("span", btn).html(p.text);
        }
        if (typeof (p.appendTo) == "string") p.appendTo = $(p.appendTo);
        btn.appendTo(p.appendTo);
    };

    // 创建过滤规则(查询表单)
    LG.bulidFilterGroup = function (form) {
        if (!form) return null;
        var group = { op: "and", rules: [] };
        var selectIgnoreName;
        $(":input", form).not(":submit, :reset, :image,:button, [disabled]")
        .each(function () {
            if (!this.name) return;
            if (this.name == selectIgnoreName) return;
            //if (!$(this).hasClass("field")) return;
            if ($(this).val() == null || $(this).val() == "") return;
            var ltype = $(this).attr("ltype");
            var optionsJSON = $(this).attr("ligerui"), options;
            if (optionsJSON) {
                options = JSON2.parse(optionsJSON);
            }
            var op = $(this).attr("op") || "like";
            //get the value type(number or date)
            var type = $(this).attr("vt") || "string";
            var value = $(this).val();
            var name = this.name;
            //如果是下拉框，那么读取下拉框关联的隐藏控件的值(ID值,常用与外表关联)
            if (ltype == "select" && options && options.valueFieldID) {
                value = $("#" + options.valueFieldID).val();
                name = selectIgnoreName = options.valueFieldID;
            }
            group.rules.push({
                op: op,
                field: name,
                value: value,
                type: type
            });
        });
        return group;
    };

    // 附加表单搜索按钮：搜索、高级搜索
    LG.appendSearchButtons = function (form, grid, queryCallback, addExcel, addSearch) {
        if (!form) return;
        form = $(form);
        //搜索按钮 附加到第一个li  高级搜索按钮附加到 第二个li
        var container = $('<ul><li style="margin-right:8px;margin-left:80px"></li><li style="margin-right:8px"></li><li></li></ul><div class="l-clear"></div>').appendTo(form);

        var excelContainer = "", searchContainer = "";
        if (addExcel) {
            excelContainer = container.find("li:eq(1)");
        }
        if (addSearch) {
            searchContainer = container.find("li:eq(2)");
        }

        LG.addSearchButtons(form, grid, queryCallback, container.find("li:eq(0)"), excelContainer, searchContainer);

    };

    // 创建表单搜索按钮：搜索、高级搜索
    LG.addSearchButtons = function (form, grid, queryCallback, btn1Container, btn2Container, btn3Container) {
        if (!form) return;
        if (btn1Container) {
            LG.createButton({
                appendTo: btn1Container,
                text: '搜索',
                click: function () {
                    var rule = LG.bulidFilterGroup(form);
                    if (queryCallback) {
                        queryCallback(rule);
                    }
                    else {
                        loadData(grid, rule);
                    }
                }
            });
        }
        if (btn2Container) {
            LG.createButton({
                appendTo: btn2Container,
                width: 80,
                text: 'Excel',
                click: function () {
                    var rule = LG.bulidFilterGroup(form);
                    rule.rules.push({
                        field: "url",
                        value: grid.get("url")
                    });
                    LG.getExcel(rule);
                }
            });
        }
        //        if (btn3Container) {
        //            LG.createButton({
        //                appendTo: btn3Container,
        //                width: 80,
        //                text: '高级搜索',
        //                click: function () {
        //                    grid.showFilter();
        //                }
        //            });
        //        }
    };

    //快速设置表单底部默认的按钮:保存、取消
    LG.setFormDefaultBtn = function (cancleCallback, savedCallback) {
        //表单底部按钮
        var buttons = [];
        if (cancleCallback) {
            buttons.push({ text: '取消', onclick: cancleCallback });
        }
        if (savedCallback) {
            buttons.push({ text: '保存', onclick: savedCallback });
        }
        LG.addFormButtons(buttons);
    };

    //增加表单底部按钮,比如：保存、取消
    LG.addFormButtons = function (buttons) {
        if (!buttons) return;
        var formbar = $("body > div.form-bar");
        if (formbar.length == 0)
            formbar = $('<div class="form-bar"><div class="form-bar-inner"></div></div>').appendTo('body');
        if (!(buttons instanceof Array)) {
            buttons = [buttons];
        }
        $(buttons).each(function (i, o) {
            var btn = $('<div class="l-dialog-btn"><div class="l-dialog-btn-l"></div><div class="l-dialog-btn-r"></div><div class="l-dialog-btn-inner"></div></div> ');
            $("div.l-dialog-btn-inner:first", btn).html(o.text || "BUTTON");
            if (o.onclick) {
                btn.bind('click', function () {
                    o.onclick(o);
                });
            }
            if (o.width) {
                btn.width(o.width);
            }
            $("> div:first", formbar).append(btn);
        });
    };

    // 填充表单数据
    LG.loadForm = function (mainform, data, prefix) {
        //根据返回的属性名，找到相应ID的表单元素，并赋值
        prefix = prefix || "";
        if (data) {
            for (var p in data) {
                var ui = $.ligerui.get(prefix + p) || $.ligerui.get(prefix + p + "_select");
                if (ui) {
                    if (ui.type == "ComboBox" && ui.options.textField != "text" && ui.options.tree == null) {
                        ui.loadText({
                            id: ui.options.valueFieldID, idfield: ui.options.valueFieldID,
                            textfield: ui.options.textField, value: data[p]
                        });
                    }
                    else {
                        ui.setValue(data[p]);
                    }
                }
                else {
                    $("#" + prefix + p).val(data[p]);
                }
            }
        }
        //下面是更新表单的样式
        var managers = $.ligerui.find($.ligerui.controls.Input);
        for (var i = 0, l = managers.length; i < l; i++) {
            //改变了表单的值，需要调用这个方法来更新ligerui样式
            managers[i].updateStyle();
        }
    };

    // 带验证、带loading的提交
    LG.submitForm = function (mainform, success, error) {
        if (!mainform)
            mainform = $("form:first");
        //验证
        jQuery.metadata.setType("attr", "validate");
        LG.validate(mainform);

        if (mainform.valid()) {
            mainform.ajaxSubmit({
                dataType: 'json',
                type: "post",
                async: false,
                success: success,
                beforeSubmit: function (formData, jqForm, options) {
                    //针对复选框和单选框 处理
                    $(":checkbox,:radio", jqForm).each(function () {
                        if (!existInFormData(formData, this.name)) {
                            formData.push({ name: this.name, type: this.type, value: this.checked });
                        }
                    });
                    for (var i = 0, l = formData.length; i < l; i++) {
                        var o = formData[i];
                        if (o.type == "checkbox" || o.type == "radio") {
                            o.value = $("[name=" + o.name + "]", jqForm)[0].checked ? "true" : "false";
                        }
                    }
                },
                beforeSend: function (a, b, c) {
                    LG.showLoading('正在保存数据中...');

                },
                complete: function () {
                    LG.hideLoading();
                },
                error: function (result) {
                    LG.tip('发现系统错误 <BR>' + result.responseText.slice(result.responseText.indexOf("<title>") + 7, result.responseText.indexOf("</title>")));
                }
            });
        }
        else {
            LG.showInvalid();
        }
        function existInFormData(formData, name) {
            for (var i = 0, l = formData.length; i < l; i++) {
                var o = formData[i];
                if (o.name == name) return true;
            }
            return false;
        }
    };

    // 提示 验证错误信息
    LG.showInvalid = function (validator) {
        validator = validator || LG.validator;
        if (!validator) return;
        var message = '<div class="invalid">存在' + validator.errorList.length + '个字段验证不通过，请检查!</div>';
        LG.tip(message);
        //$.ligerDialog.error(message);
    };

    // 表单验证
    LG.validate = function (form, options) {
        if (typeof (form) == "string")
            form = $(form);
        else if (typeof (form) == "object" && form.NodeType == 1)
            form = $(form);

        options = $.extend({
            errorPlacement: function (lable, element) {
                if (!element.attr("id"))
                    element.attr("id", new Date().getTime());
                if (element.hasClass("l-textarea")) {
                    element.addClass("l-textarea-invalid");
                }
                else if (element.hasClass("l-text-field")) {
                    element.parent().addClass("l-text-invalid");
                }
                $(element).removeAttr("title").ligerHideTip();
                $(element).attr("title", lable.html()).ligerTip({
                    distanceX: 5,
                    distanceY: -3,
                    auto: true
                });
            },
            success: function (lable) {
                if (!lable.attr("for")) return;
                var element = $("#" + lable.attr("for"));

                if (element.hasClass("l-textarea")) {
                    element.removeClass("l-textarea-invalid");
                }
                else if (element.hasClass("l-text-field")) {
                    element.parent().removeClass("l-text-invalid");
                }
                $(element).removeAttr("title").ligerHideTip();
            }
        }, options || {});
        LG.validator = form.validate(options);
        return LG.validator;
    };

    LG.loadToolbar = function (grid, toolbarBtnItemClick) {
        LG.ajax({
            loading: '正在加载工具条中...',
            url: "/config/moduleinfo",
            data: { id: moduleNo },
            success: function (data) {
                if (!grid.toolbarManager) return;
                if (!data || !data.length) return;
                var items = [];
                for (var i = 0, l = data.length; i < l; i++) {
                    var o = data[i];
                    if (o.Action == "query") {
                        continue;
                    }
                    items[items.length] = {
                        click: toolbarBtnItemClick,
                        text: o.Text,
                        img: o.Icon,
                        id: o.Action
                    };
                    items[items.length] = { line: true };
                }
                grid.toolbarManager.set('items', items);
            }
        });
    };

    // 覆盖页面grid的loading效果
    LG.overrideGridLoading = function () {
        $.extend($.ligerDefaults.Grid, {
            onloading: function () {
                LG.showLoading('正在加载表格数据中...');
            },
            onloaded: function () {
                LG.hideLoading();
            }
        });
    };

    // 根据字段权限调整 页面配置
    LG.adujestConfig = function (config, forbidFields) {
        if (config.Form && config.Form.fields) {
            for (var i = config.Form.fields.length - 1; i >= 0; i--) {
                var field = config.Form.fields[i];
                if ($.inArray(field.name, forbidFields) != -1)
                    config.Form.fields.splice(i, 1);
            }
        }
        if (config.Grid && config.Grid.columns) {
            for (var i = config.Grid.columns.length - 1; i >= 0; i--) {
                var column = config.Grid.columns[i];
                if ($.inArray(column.name, forbidFields) != -1)
                    config.Grid.columns.splice(i, 1);
            }
        }
        if (config.Search && config.Search.fields) {
            for (var i = config.Search.fields.length - 1; i >= 0; i--) {
                var field = config.Search.fields[i];
                if ($.inArray(field.name, forbidFields) != -1)
                    config.Search.fields.splice(i, 1);
            }
        }
    };

    // 查找是否存在某一个按钮
    LG.findToolbarItem = function (grid, itemID) {
        if (!grid.toolbarManager) return null;
        if (!grid.toolbarManager.options.items) return null;
        var items = grid.toolbarManager.options.items;
        for (var i = 0, l = items.length; i < l; i++) {
            if (items[i].id == itemID) return items[i];
        }
        return null;
    }


    // 设置grid的双击事件(带权限控制)
    LG.setGridDoubleClick = function (grid, btnID, btnItemClick) {
        btnItemClick = btnItemClick || toolbarBtnItemClick;
        if (!btnItemClick) return;
        grid.bind('dblClickRow', function (rowdata) {
            var item = LG.findToolbarItem(grid, btnID);
            if (!item) return;
            grid.select(rowdata);
            btnItemClick(item);
        });
    }


    // changepassword
    LG.changepassword = function () {
        $(document).bind('keydown.changepassword', function (e) {
            if (e.keyCode == 13) {
                doChangePassword();
            }
        });

        if (!window.changePasswordWin) {
            var changePasswordPanle = $("<form></form>");
            changePasswordPanle.ligerForm({
                fields: [
                { display: '旧密码', name: 'OldPassword', type: 'password', validate: { maxlength: 50, required: true, messages: { required: '请输入密码' } } },
                { display: '新密码', name: 'NewPassword', type: 'password', validate: { maxlength: 50, required: true, messages: { required: '请输入密码' } } },
                { display: '确认密码', name: 'NewPassword2', type: 'password', validate: { maxlength: 50, required: true, equalTo: '#NewPassword', messages: { required: '请输入密码', equalTo: '两次密码输入不一致' } } }
                ]
            });

            //验证
            //jQuery.metadata.setType("attr", "validate");
            LG.validate(changePasswordPanle);

            window.changePasswordWin = $.ligerDialog.open({
                width: 400,
                height: 190, top: 200,
                isResize: true,
                title: '用户修改密码',
                target: changePasswordPanle,
                buttons: [
            {
                text: '确定', onclick: function () {
                    doChangePassword();
                }
            },
            {
                text: '取消', onclick: function () {
                    window.changePasswordWin.hide();
                    $(document).unbind('keydown.changepassword');
                }
            }
                ]
            });
        }
        else {
            window.changePasswordWin.show();
        }

        function doChangePassword() {
            var OldPassword = $("#OldPassword").val();
            var LoginPassword = $("#NewPassword").val();
            if (changePasswordPanle.valid()) {
                LG.ajax({
                    url: "/home/changepassword",
                    data: { OldPassword: OldPassword, LoginPassword: LoginPassword },
                    success: function () {
                        LG.showSuccess('密码修改成功');
                        window.changePasswordWin.hide();
                        $(document).unbind('keydown.changepassword');
                    },
                    error: function (message) {
                        LG.showError(message);
                    }
                });
            }
        }
    }

    // changeuser
    LG.changeuser = function () {
        $(document).bind('keydown.changeuser', function (e) {
            if (e.keyCode == 13) {
                doChangeUser();
            }
        });

        if (!window.changeuserWin) {
            var changeuserPanle = $("<form></form>");
            changeuserPanle.ligerForm({
                fields: [
                { display: '用户名', name: 'UserName', type: 'text', validate: { maxlength: 50, required: true, messages: { required: '请输入用户名' } } },
                { display: '密  码', name: 'Password', type: 'text', validate: { maxlength: 50, required: true, messages: { required: '请输入密码' } } },
                //{ display: '确认密码', name: 'NewPassword2', type: 'password', validate: { maxlength: 50, required: true, equalTo: '#NewPassword', messages: { required: '请输入密码', equalTo: '两次密码输入不一致' } } }
                ]
            });

            //验证
            //jQuery.metadata.setType("attr", "validate");
            LG.validate(changeuserPanle);

            window.changeuserWin = $.ligerDialog.open({
                width: 400,
                height: 190, top: 200,
                isResize: true,
                title: '切换登录用户',
                target: changeuserPanle,
                buttons: [
            {
                text: '确定', onclick: function () {
                    doChangeUser();
                }
            },
            {
                text: '取消', onclick: function () {
                    window.changeuserWin.hide();
                    $(document).unbind('keydown.changeuser');
                }
            }
                ]
            });
        }
        else {
            window.changeuserWin.show();
        }

        function doChangeUser() {
            var UserName = $("#UserName").val();
            var Password = $("#Password").val();
            if (changeuserPanle.valid()) {
                LG.ajax({
                    url: "/home/login",
                    data: [
                   { name: 'username', value: UserName },
                   { name: 'password', value: Password }
                    ],
                    success: function () {
                        LG.showSuccess('切换用户成功');
                        window.changePasswordWin.hide();
                        $(document).unbind('keydown.changepassword');
                        location.href = "/home/main";
                    },
                    error: function (message) {
                        LG.showError(message);
                    }
                });
            }
        }
    }






    //设置弹出form
    LG.showDialogForm = function (option) {
        var p = $.extend(true, {}, option);

        var isFrame = false;
        $.ajax({
            url: p.action,
            type: "GET",
            dataType: "html",
            async: false,
            success: function (data) { isFrame = true; }
        });

        //if ($.ligerui.get("dialog")) {
        //    var d = $.ligerui.get("dialog");
        //    if (isFrame) {
        //        if (p.data) {
        //            d.setUrl(p.action + "?" + $.param(p.data));
        //        }
        //        else {
        //            d.setUrl(p.action);
        //        }
        //    }
        //    else {
        //        $("form", "div.l-dialog").attr("action", p.action);
        //        if (p.data) {
        //            LG.loadForm($("form", "div.l-dialog"), p.data);
        //        }
        //        else {
        //            $("form", ".l-dialog")[0].reset();
        //        }
        //    }
        //    d.show();
        //    return;
        //}

        var form = $('<form></form> ');
        var buttons = [];
        if (p.action) {
            buttons.push({
                text: '保存', onclick: function (item, dialog) {
                    if (isFrame) {
                        if (dialog.frame.save) {
                            dialog.frame.save(dialog);
                        }
                        //通过触发搜索按键事件,刷新数据
                        if (p.success) {
                            p.success();
                        }
                    }
                    else {
                        LG.submitForm(form, function (data) {
                            if (!data.IsError) { LG.tip("保存成功!"); dialog.close(); }
                            else { LG.showError(data.Message); }
                            if (p.success) {
                                p.success(data);
                            }
                        });
                    }
                }
            });
        }
        buttons.push({ text: '关闭', onclick: function (item, dialog) { dialog.close(); } });



        var dialogOptions = {
            title: p.title || ' 明细 界面',
            width: p.width || 600, height: p.height, isResize: true,
            showMax: true,
            buttons: buttons
        }
        if (isFrame) {
            $.extend(true, dialogOptions, { width: p.width || 750, height: p.height || 550 });
            if (p.data) {
                $.extend(true, dialogOptions, { url: p.action + "?" + $.param(p.data) });
            }
            else {
                $.extend(true, dialogOptions, { url: p.action });
            }
        }
        else {
            if (p.action) {
                $(form).attr("action", p.action);
            }
            $(form).attr("method", "post");

            //删除管理器里的id
            for (var i = 0; i < p.fields.length; i++) {
                if (p.fields[i].comboboxName) {
                    LG.clear(p.fields[i].comboboxName);
                }
                else {
                    LG.clear(p.fields[i].name);
                }
            }

            form.ligerForm({ fields: p.fields });
            $.extend(true, dialogOptions, { target: form });


        }
        $.ligerDialog.open(dialogOptions);

        if (p.data) {
            LG.loadForm(form, p.data);
        }

    }

    //清理管理器
    LG.clear = function (name) {
        var managers = $.ligerui.find($.ligerui.controls.Input);
        for (var i = 0, l = managers.length; i < l; i++) {
            if (managers[i].id == name) {
                managers[i].destroy();
            }
        }
    }

    //获取 表单和表格 结构 所需要的数据
    LG.bulidData = function (columns, data) {
        var griddata = [], searchdata = [], formdata = [], treeColumn, fields = []; chartX = []; chartY = [], total = [], _columns = [];

        if (!$.isArray(columns)) {
            _columns.push(columns);
        }
        else {
            _columns = columns;
        }

        //有data,默认为查询,无配置,取默认配置
        if (data) {
            $.each(data, function (i, n) { fields.push(i); });
        }
            //有配置,取配置
        else {
            $.each(_columns, function (i, n) {
                if (n.ModuleNo != "default") {
                    fields.push(n.Name);
                }
            });
        }

        for (var i = 0, l = fields.length; i < l; i++) {
            var o = exits(fields[i]);
            if (o.IsTreeColumn) {
                treeColumn = o;
            }
            if (o.InList)
                griddata.push(getListData(o, fields[i]));
            if (o.InForm) {
                formdata.push(getFieldData(o));
            }
            else {
                formdata.push({ name: o.Name, type: "hidden" });
            }

        }

        for (var i = 0, l = _columns.length; i < l; i++) {
            var o = _columns[i];
            if (o.InSearch && o.ModuleNo != 'default')
                searchdata.push(getFieldData(o, true));
        }


        function exits(id) {
            for (var i = 0, l = _columns.length; i < l; i++) {
                if (data && _columns[i].ModuleNo != 'default') continue;
                var name = _columns[i].Name.toLowerCase();
                if (name == id.toLowerCase()) return _columns[i];
            }
            return {
                "Name": id,
                "Text": id,
                "Type": "text",
                "IsInPrimaryKey": false,
                "IsInForeignKey": false,
                "IsNullable": true,
                "IsAutoKey": false,
                "IsTreeColumn": false,
                "SourceTableName": "",
                "SourceTableIDField": "",
                "SourceTableTextField": "",
                "Align": "",
                "InList": true,
                "ListWidth": 80,
                "InSearch": false,
                "Search_NewLine": false,
                "InForm": true,
                "NewLine": false,
                "Group": "",
                "Index": 99
            }
        }


        return { grid: griddata, search: searchdata, form: formdata, treeColumn: treeColumn, chartX: chartX, chartY: chartY, total: total, totalRender: totalRender };

        function getFieldData(o, search) {
            if (o.type == "hidden") return { name: o.Name, type: o.Type };
            var field = {
                display: o.Text == null ? o.Name : o.Text,
                name: o.Name + (search ? "_search" : ""),
                newline: (o.NewLine == null || o.NewLine == "null") ? false : o.NewLine,
                labelWidth: o.labelwidth || o.labelWidth || 80,
                width: o.width || 180,
                space: o.space || 20,
                type: o.Type
            };
            if (o.Group) {
                field.group = o.Group;
                field.groupicon = "/content/icons/communication.gif";
            }
            if (!search) {
                field.validate = getValidate(o);
                field.group = (o.Group == null || o.Group == "null") ? "" : o.Group;
                field.groupicon = "/content/icons/communication.gif";
            }
            else {
                field.cssClass = "field";
                field.newline = o.Search_NewLine ? true : false;
                if (field.type == "label") {
                    field.type = "text";
                }
            }
            if (field.type == "textarea") {
                $.extend(true, field, { width: 460 });
            }

            //select
            if (field.type == "select") {
                //$.extend(true, field, { options: { url: "/config/select/" + o.Name, valueFieldID: field.name }, comboboxName: field.name + "_select" });
                $.extend(true, field,
                    {
                        options:
                          {
                              url: "/config/select/" + moduleNo + "_" + o.Name,
                              valueFieldID: field.name
                              //valueFieldID: field.name+field.ModuleNo
                          },
                        comboboxName: field.name + "_select"
                    });
            }

            if (field.type == "selectpage") {
                $.extend(field, {
                    type: "select", options: {
                        onBeforeOpen: function () {
                            LG.showSelect("/config/Columns/" + o.SourceTableName,
                                "/manage/Get" + o.SourceTableName, this.valueField[0].form); return false;
                        }, valueFieldID: field.name, textField: o.SourceTableTextField
                    },
                    comboboxName: field.name + "_select"
                });
            }
            //tree
            if (field.type == "tree") {
                $.extend(true, field, {
                    type: "select", name: field.name + "_select",
                    options: {
                        tree: {
                            url: "/config/select/" + o.Name,
                            idFieldName: o.SourceTableIDField,
                            parentIDFieldName: o.SourceTableParentIDField,
                            textFieldName: o.SourceTableTextField,
                            checkbox: false
                        }, valueFieldID: o.Name,
                        valueField: o.SourceTableIDField,
                        textField: o.SourceTableTextField,
                        treeLeafOnly: false
                    }
                });
            }
            return field;
        }
        function getValidate(o) {
            if (o.validate) return o.validate;
            if (!o.IsNullable) return { required: true };
            return null;
        }

        function getListData(o, name) {

            var field = {
                display: o.Text == null ? name : o.Text,
                name: name,
                width: o.ListWidth,
                type: o.Type,
                align: o.Align
            };

            if ($.inArray(field.type, ["currency", "percent", "number"]) >= 0) {
                chartY.push({ id: field.name, text: field.display });
                if (field.type != "percent") total.push({ name: field.name, text: field.display });

                //                $.extend(true, field, { totalSummary: {
                //                    render: function (suminf, column, cell) {
                //                        return '<div>合计:' + suminf.sum + '</div>';
                //                    },
                //                    align: 'right'
                //                }
                //                });

                $.extend(true, field, { align: "right", editor: { type: field.type } });
            }

            if ($.inArray(field.type, ["text"]) >= 0) {
                chartX.push({ id: field.name, text: field.display });
            }

            return field;
        }

        function totalRender(data, currentData) {
            var d = data.Rows;
            if (total.length <= 0) return "";

            var s = [];
            for (var j = 0; j < total.length; j++) {
                var sum = 0; count = 0;
                for (var i = 0; i < d.length; i++) {
                    count++;
                    sum = sum + d[i][total[j].name];
                }
                if (total[j].text.indexOf("客单价") >= 0 || total[j].text.indexOf("客单数") >= 0) {
                    s.push(total[j].text + " 平均:  <b>" + (sum / count).toFixed(2) + "</b>    ");
                }
                else {
                    s.push(total[j].text + " 合计:  <b>" + sum.toFixed(2) + "</b>    ");
                }
            }

            return s.join(" ");
        }
    }

    //获取excel
    LG.getExcel = function (data) {
        var pa = [];
        $.each(data.rules, function () {
            pa.push({ name: this.field.replace("_search", ""), value: this.value });
        });
        window.open("/manage/getexcel?" + $.param(pa));
    }


    //全局事件
    $(".l-dialog-btn").on('mouseover', function () {
        $(this).addClass("l-dialog-btn-over");
    }).on('mouseout', function () {
        $(this).removeClass("l-dialog-btn-over");
    });
    $(".l-dialog-tc .l-dialog-close").on('mouseover', function () {
        $(this).addClass("l-dialog-close-over");
    }).on('mouseout', function () {
        $(this).removeClass("l-dialog-close-over");
    });
    //搜索框 收缩/展开
    $(".searchtitle .togglebtn").on('click', function () {
        if ($(this).hasClass("togglebtn-down")) $(this).removeClass("togglebtn-down");
        else $(this).addClass("togglebtn-down");
        var searchbox = $(this).parent().nextAll("div.searchbox:first");
        searchbox.slideToggle('fast');
    });

    //弹出ajax错误消息
    //    $(document).ajaxError(function (event, request, settings) {
    //        LG.hideLoading();
    //        LG.tip(request.responseText.slice(request.responseText.indexOf("<title>") + 7, request.responseText.indexOf("</title>")));
    //    });

    LG.showSelect = function (configUrl, dataUrl, select, checkbox) {
        $.getJSON(configUrl, function (data) {
            var config = LG.bulidData(data);
            var panel = $("<div></div>");
            var searchForm = $("<form></form>");
            var gridPanel = $("<div></div>");

            panel.append(searchForm).append(gridPanel);

            //grid
            var grid = gridPanel.ligerGrid({
                columns: config.grid,
                pageSize: 10,
                url: dataUrl,
                width: '98%', checkbox: checkbox ? true : false, isResize: true
            });
            //dialog
            $.ligerDialog.open({
                title: '选择',
                width: 600, height: 460,
                target: panel,
                buttons: [
             {
                 text: '选择', onclick: function (item, dialog) {
                     if ($.isFunction(select)) {
                         select(grid.getSelecteds());
                     }
                     else {
                         LG.loadForm(select, grid.getSelected());
                     }
                     dialog.close();
                 }
             },
             { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });

            //搜索
            //清除管理器id
            for (var i = 0; i < config.search.length; i++) {
                LG.clear(config.search[i].name);
            }
            searchForm.ligerForm({
                fields: config.search,
                toJSON: JSON2.stringify
            });

            //增加搜索按钮,并创建事件
            LG.appendSearchButtons(searchForm, grid);

        });
    }
})(jQuery);

