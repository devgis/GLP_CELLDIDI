$("#mapDiv").jsGrid({
    width: "100%",
    height: "100%",

    //inserting: true,
    //editing: true,
    //sorting: true,
    paging: true,
    pagerFormat: "页码: {first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount}",
    pagePrevText: "下一页",
    pageNextText: "前一页",
    noDataContent: "加载中，请稍等。。。",
    pageFirstText: "首页",
    pageLastText: "尾页",
    itemTemplate: function (value, item) { console.log(value); console.log(item); },
    fields: [
            { name: "orderid", title: "订单号", type: "text", validate: "required", width: 160 },
            { name: "stime", title: "开始时间", type: "date" },
            { name: "etime", title: "结束时间", type: "date" },
            { name: "x1", title: "开始经度", type: "number", width: 35 },
            { name: "y1", title: "开始纬度", type: "number", width: 35 },
            { name: "x2", title: "结束经度", type: "number", width: 35 },
            { name: "y2", title: "结束纬度", type: "number", width: 35 }
                ]
});

//请求数据
//从后台获取gps数据
var dataurl="./data.ashx?t=data";
$.ajax({
    type: "get",
    //contentType: "application/json",
    url: dataurl,
    //data: "{}",  //这里是要传递的参数，格式为 data: "{paraName:paraValue}",下面将会看到       
    dataType: 'json',   //WebService 会返回Json类型
    success: function (result) {     //回调函数，result，返回值
        if (result.length > 0) {
            var jsgrid = $("#mapDiv").jsGrid({
                width: "100%",
                height: "100%",
                noDataContent: "无数据。。。",
                //inserting: true,
                //editing: true,
                //sorting: true,
                paging: true,
                pagerFormat: "页码: {first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount}",
                pagePrevText: "下一页",
                pageNextText: "前一页",
                pageFirstText: "首页",
                pageLastText: "尾页",
                itemTemplate: function (value, item) { console.log(value); console.log(item); },
                data: result,
                fields: [
            { name: "orderid", title: "订单号", type: "text", validate: "required", width: 160 },
            { name: "stime", title: "开始时间", type: "date" },
            { name: "etime", title: "结束时间", type: "date" },
            { name: "x1", title: "开始经度", type: "number", width: 35 },
            { name: "y1", title: "开始纬度", type: "number", width: 35 },
            { name: "x2", title: "结束经度", type: "number", width: 35 },
            { name: "y2", title: "结束纬度", type: "number", width: 35 }
                ]
            });

            //            var MyDateField = function (config) {
            //                jsgrid.Field.call(this, config);
            //            };

            //            MyDateField.prototype = new jsGrid.Field({

            //                css: "date-field",            // redefine general property 'css'
            //                align: "center",              // redefine general property 'align'

            //                myCustomProperty: "foo",      // custom property

            //                itemTemplate: function (value) {
            //                    //return new Date(value).toDateString();
            //                    return "";
            //                }
            //            });

            //            jsgrid.fields.date = MyDateField;

        }
        else {
            $("#mapDiv").jsGrid({
                width: "100%",
                height: "100%",

                //inserting: true,
                //editing: true,
                //sorting: true,
                paging: true,
                pagerFormat: "页码: {first} {prev} {pages} {next} {last}    {pageIndex} of {pageCount}",
                pagePrevText: "下一页",
                pageNextText: "前一页",
                noDataContent: "加载中，请稍等。。。",
                pageFirstText: "首页",
                pageLastText: "尾页",
                itemTemplate: function (value, item) { console.log(value); console.log(item); },
                fields: [
            { name: "orderid", title: "订单号", type: "text", validate: "required", width: 160 },
            { name: "stime", title: "开始时间", type: "date" },
            { name: "etime", title: "结束时间", type: "date" },
            { name: "x1", title: "开始经度", type: "number", width: 35 },
            { name: "y1", title: "开始纬度", type: "number", width: 35 },
            { name: "x2", title: "结束经度", type: "number", width: 35 },
            { name: "y2", title: "结束纬度", type: "number", width: 35 }
                ]
            });
        }
        //alert("ok");
    },
    error: function (err) {     //  出错s
        console.log(err);
    }
});