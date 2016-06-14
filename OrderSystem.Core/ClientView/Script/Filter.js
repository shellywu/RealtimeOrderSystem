(function (app, $) {
    app.filter("orderState", function () {
        return function (os) {
            var s = "";
            switch (os) {
                case 0: s = "创建订单"; break;
                case 1: s = "创建订单"; break;
                case 2: s = "处理中"; break;
                case 3: s = "完成订单"; break;
                case 4: s = "转单"; break;
                case 5: s = "部分完成"; break;
                case 6: s = "订单失败"; break;
                case 7: s = "生成取消单"; break;
                case 8: s = "订单取消中"; break;
                case 9: s = "退单完成"; break;
                case 10: s = "退单失败"; break;
                default:

            }
            return s;
        }
    });

    app.filter("productType", function () {
        return function (ts) {
            var tsr = "";
            switch (ts) {
                case 1: tsr = "门票"; break;
                case 2: tsr = "酒店"; break;
                case 4: tsr = "租车"; break;
            }
            return tsr;
        }
    });

    app.filter("taskState", function () {
        return function (ts) {
            var tsr = "";
            switch (ts) {
                case 0: tsr = "创建任务"; break;
                case 1: tsr = "已领取"; break;
                case 2: tsr = "已完成"; break;
                case 3: tsr = "修改订单"; break;
                case 4: tsr = "下单失败"; break;
                case 7: tsr = "待取消"; break;
                case 8: tsr = "取消中"; break;
                case 9: tsr = "取消完成"; break;
                case 10: tsr = "取消失败"; break;
            }
            return tsr;
        }
    })

    app.filter("remarkFormter", function () {
        return function (ts) {
            var tsr = "";
            tsr = ts.replace(/\r\n/g, " | ")
            return tsr;
        }
    })
})(angular.module("orderSys"), jQuery)