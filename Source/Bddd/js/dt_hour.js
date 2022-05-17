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
            { name: "startrectid", title: "开始小区", type: "text" },
            { name: "endrectid", title: "结束小区", type: "text" },
            { name: "power", title: "数量", type: "number", width: 35 }
                ]
});

var starth = GetQueryString("shour");
var endh = GetQueryString("ehour");

//请求数据
//从后台获取gps数据
var dataurl = "./data.ashx?xcout=" + colcount + "&ycout=" + rowcount + "&startx=" + startx + "&endx=" + endx + "&starty=" + starty + "&endy=" + endy + "&t=byhour&shour="+starth+"&ehour="+endh;
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
            { name: "startrectid", title: "开始小区", type: "text" },
            { name: "endrectid", title: "结束小区", type: "text" },
            { name: "power", title: "数量", type: "number", width: 35 }
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
                noDataContent: "无数据。。。",
                pageFirstText: "首页",
                pageLastText: "尾页",
                itemTemplate: function (value, item) { console.log(value); console.log(item); },
                fields: [
            { name: "startrectid", title: "开始小区", type: "text" },
            { name: "endrectid", title: "结束小区", type: "text" },
            { name: "power", title: "数量", type: "number", width: 35 }
                ]
            });
        }
        //alert("ok");
    },
    error: function (err) {     //  出错s
        console.log(err);
    }
});