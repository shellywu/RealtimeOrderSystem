///  <reference path="../../Scripts/angular.js" />
/// <reference path="../../Scripts/angular-ui/ui-bootstrap.js" />
/// <reference path="../Scripts/angular.intellisense.js" />

(function (app) {
    var UserInfo = function ($http, userSrv) {
        var getUserRoles = function () {
            return $http.get(userSrv);
        }
        return {
        getUserRoles:getUserRoles
        }
    }

    var TaskInfo = function ($http, taskSrv, taskSrvPersonal) {
        var getTasks = function (querys) {
            return $http.get(taskSrv + "/values?rowCount=" + querys.rowCount + "&pageSize=" + querys.pageSize + "&pt=" + querys.pt);
        }

        var getPersonelTasks = function (querys) {
            return $http.get(taskSrvPersonal + "/values?rowCount=" + querys.rowCount + "&pageSize=" + querys.pageSize + "&pt=" + querys.pt);
        }

        var updateTask = function (id, state, msg) {
            var updateModel = { id: id, state: state, remark: msg };
            return $http.put(taskSrv,updateModel);
        }

        return {
            getTasks: getTasks,
            getMyTasks:getPersonelTasks,
            updateTask: updateTask
        }
    }
         

    var OrderService = function ($http, orderSrvUrl, orderCancelUrl,orderChangeUrl) {
        var getById = function (id) {
            return $http.get(orderCancelUrl + id);
        };

        var getItemsByOrderId = function (id) {
            return $http.get(orderChangeUrl+"/"+ id);
        };

        var getDetailItemById = function (id) {
            return $http.put(orderChangeUrl+"/"+id);
        }

        var modifyOrderItem = function (changeOrderItem) {
            return $http.post(orderChangeUrl,changeOrderItem);
        }

        var getByQuery = function (model) {
           
            return $http.post(orderCancelUrl,model);
        }
       
        var cancelOrder = function (id) {
            return $http.put(orderCancelUrl+id);
        }

        var getAllOrder = function (pageInfo) {
            return $http.get(orderSrvUrl + "values?rowCount="+pageInfo.rowCount+"&pageSize="+pageInfo.pageSize+"&userId="+pageInfo.userId);
             
        }
        var getByType = function (type) {
            return $http.get(orderSrvUrl + type);
        };



        var createOrder = function (order) {
            return $http.post(orderSrvUrl,order);
        };

        var editOrder = function (order) {
            $http.put(orderSrvUrl, order);
        };

        return {
            getById: getById,
            getByQuery: getByQuery,
            cancelOrder:cancelOrder,
            gerAllOrder:getAllOrder,
            getByType: getByType,
            createOrder: createOrder,
            editOrder: editOrder,
            getItemsByOrderId: getItemsByOrderId,
            getDetailItemById: getDetailItemById,
            modifyOrderItem:modifyOrderItem
        }
    }

    var CustomerService=function($http,customerSrvUrl){
        var getByPhone = function (phone) {
            return $http.get(customerSrvUrl+phone);
        }

        var createCustomer = function (customerInfo) {
           return $http.post(customerSrvUrl, customerInfo);
        }

        var editCustomer = function (customerInfo) {
           return $http.put(customerSrvUrl, customerInfo);
        }

        return {
            getByPhone: getByPhone,
            createCustomer: createCustomer,
            editCustomer:editCustomer
        }
    }

    var productService = ["$http", "productSrvUrl", function ($http, productSrvUrl) {
        var getByCodeOrName = function (code, name) {
            var productQueryModel = { ProductCode: code, ProductName: name };
            return $http.post(productSrvUrl,productQueryModel);
        }

        return {
            getByCodeOrName: getByCodeOrName,
        }
    }]

    var comboService = ["$http", "comboProductSrvUrl", function ($http, comboProductSrvUrl) {
        var getByCodeOrName = function (code, name) {
            var comboQueryModel = { ProductCode: code, ProductName: name };
            return $http.post(comboProductSrvUrl ,comboQueryModel);
        }

        return {
            getByCodeOrName: getByCodeOrName,
        }
    }]

    var notifyService = ["$http", "notifySerUrl", function ($http, notifySerUrl) {
        var getAudio = function (msg) {
            $("#notify").attr("src",notifySerUrl+"?msg="+msg);
        }
        return {
        getAudio:getAudio
        }
    }];

    app.service("CustomerService", CustomerService);
    app.service("ProductService", productService);
    app.service("ComboService", comboService);
    app.service("OrderService", OrderService);
    app.service("$UserInfo", UserInfo);
    app.service("$TaskInfo", TaskInfo);
    app.service("notifyService",notifyService);
})(angular.module("orderSys"))