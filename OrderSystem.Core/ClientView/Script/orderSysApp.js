///  <reference path="/Scripts/angular.js" />
/// <reference path="/Scripts/angular-ui/ui-bootstrap.js" />

(function () {
    var signalRModule = angular.module("signalR", []);
    function $connection() {
        var connect = $.connection("/OrderInfo");

        this.received = function () { };

        this.stateChange = function () { };

        var send = function (data) {
            connect.send(data);
        }
        this.status = false;
        var run = function () {
            connect.received(this.received)
            connect.start()
                 .done(function () { this.status = true; })
                 .fail(function () { this.status = false; });
            connect.stateChanged(this.stateChange);
        }
        var setReceived = function (callback) {
            this.received = callback;
        }

        var setState = function (callback) {
            this.stateChange=callback;
        }

        return {
            setReceived: setReceived,
            setStateChange:setState,
            send: send,
            run: run,
        }
    }
    function $hub() {

        var hub = $.connection.chatHub;
        
        var received = function () { };

        var send =hub.server.send;

        var run = function () {
            $.connection.hub.start()
                 .done(send)
                 .fail(function () { this.status = false; })
        }
        hub.client.reciveMsg =function(msg){
            received(msg);
        } 

        var setReceived = function (callback) {
            received = callback;
        }
        return {
            setReceived: setReceived,
            getSend: send,
            run: run,
        }
    }
    signalRModule.service("$connection", $connection);
    signalRModule.service("$hub", $hub);

})();

(function () {
    var app = angular.module('orderSys', ["ngRoute", "ui.bootstrap", "ui.bootstrap.tpls", "signalR"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/", {
                controller: 'UIRouterCtrl',
                templateUrl: "/ClientView/View/Layout.html"
            })
            .when("/addOrder", {
                templateUrl: "/client/views/AddOrder.html"
            })
            .when(
                "/details/:id",
                { templateUrl: "/client/views/details.html" })
            .otherwise(
                { redirectTo: "/" });
    };

    app.constant("taskType", { Titcket: 2, Hotel: 3, Tax: 4, Travel: 5 });
    app.constant("customerSrvUrl", "/api/customers/");
    app.constant("productSrvUrl", "/api/products/");
    app.constant("comboProductSrvUrl", "/api/comboProducts/");
    app.constant("orderSrvUrl", "/api/orders/");
    app.constant("orderCancelUrl", "/api/CancelProcess/");
    app.constant("orderChangeUrl", "/api/OrderChange");
    app.constant("userSrv", "/api/UserInfo");
    app.constant("taskSrv", "/api/TaskInfo");
    app.constant("taskSrvPersonal", "/api/MyTaskInfo");
    app.constant("notifySerUrl", "/NotifyAudio/Index");

    app.config(config);
})();



