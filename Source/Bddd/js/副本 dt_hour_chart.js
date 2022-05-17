var dom = document.getElementById("mapDiv");
var myChart = echarts.init(dom);

var option = {
    title: {
        text: '滴滴订单分布图',
        subtext: '',
        x: 'center'
    },
    tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b} : {c} ({d}%)"
    },
    legend: {
        orient: 'vertical',
        left: 'left',
        data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎']
    },
    series: [
        {
            name: '访问来源',
            type: 'pie',
            radius: '55%',
            center: ['50%', '60%'],
            data: [
                { value: 335, name: '直接访问' },
                { value: 310, name: '邮件营销' },
                { value: 234, name: '联盟广告' },
                { value: 135, name: '视频广告' },
                { value: 1548, name: '搜索引擎' }
            ],
            itemStyle: {
                emphasis: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }
        }
    ]
};
myChart.showLoading();

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

            option.legend.data = new Array();
            option.series[0].data = new Array();
            for (var i = 0; i < result.length; i++) {
                var title = result[i].startrectid + "-" + result[i].endrectid;
                option.legend.data.push(title);
                option.series[0].data.push({ value: result[i].power, name: title });
            }
            myChart.hideLoading();
            myChart.setOption(option);

        }
        else {
            myChart.hideLoading();
        }
    },
    error: function (err) {     //  出错s
        console.log(err);
        myChart.hideLoading();
    }
});