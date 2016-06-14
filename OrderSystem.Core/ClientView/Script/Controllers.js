
///  <reference path="../../Scripts/angular.js" />
/// <reference path="../../Scripts/angular-ui/ui-bootstrap.js" />
/// <reference path="../../Scripts/angular.intellisense.js" />

(function (app, $) {
    /*********************
    * 控制器使用模型初始化
    *********************/
    var Msg = function () {
        this.id = "";
        this.createDate = "";
        this.from = "";
        this.to = "";
        this.content = "";
    }
    var ProductUserEmpty = function (index) {
        this.index = index;
        this.personName = "";
        this.personPhone = "";
        this.personIdentity = "";
        this.credentialType = 1;
    }
    /*****************************
    * 公用方法
    *****************************/

    function getItemIndex(serach, fild, arr) {
        for (var i in arr) {
            if (arr[i][fild] == serach) {
                return i;
            }
        }
        return -1;
    }

    /**************************
    * 控制器
    **************************/

    var UIRouterCtrl = ["$scope", "$UserInfo", function ($scope, $UserInfo) {
        /****************
        * 初始化ctrl数据
        ****************/
        $scope.tabs = [];
        $scope.currentTab = 1;
        $scope.minChat = function () {
            $scope.showchart = !$scope.showchart;
        }

        $scope.showchart = false;
        var activeTabEvent = function (tabIndex) {
            $scope.currentTab = tabIndex;
        }

        var getTempName = function (tid) {
            var ttname = "";
            switch (tid) {
                case "CustomerSrv": ttname = "CustomerSrv.html"; break;
                case "Tiket": ttname = "Tiket.html"; break;
                case "Tax": ttname = "Tax.html"; break;
                case "Hotel": ttname = "Hotel.html"; break;
            }
            return ttname;
        }

        var getTabName = function (tid) {
            var tname = "";
            switch (tid) {
                case "CustomerSrv": tname = "客服"; break;
                case "Tiket": tname = "门票"; break;
                case "Tax": tname = "租车"; break;
                case "Hotel": tname = "酒店"; break;
            }
            return tname;
        }

        $scope.activeTab = activeTabEvent;

        $UserInfo.getUserRoles().success(function (data) {
            var comomPath = "/ClientView/View/";
            for (var i in data) {

                if (data[i] == "admin") {
                    $scope.tabs = [{ id: "CustomerSrv", name: "客服组", index: 1, template: comomPath + "CustomerSrv.html" }
                        , { id: "Hotel", name: "酒店组", index: 2, template: comomPath + "Hotel.html" }
                        , { id: "Tax", name: "租车组", index: 3, template: comomPath + "Tax.html" }
                        , { id: "Tiket", name: "门票组", index: 4, template: comomPath + "Tiket.html" }];
                    break;
                }
                else {
                    var tab = { id: data[i], name: getTabName(data[i]), index: i + 1, template: comomPath + getTempName(data[i]) };
                    $scope.tabs.push(tab);
                }
            }
        });

        $scope.template = {
            "workSheet": "/ClientView/View/worksheet.html",
            "chart": "/ClientView/View/Chart.html"
        };
    }];

    var TaskAdapterCtrl = ["$scope", "$connection", "notifyService", function ($scope, $connection, $TaskInfo, notifyService) {
        $scope.orderList = [];
        $scope.tasks = [];
        $scope.state = "disconnect";
        var receiveOrder = function (data) {
            var datan = {};
            if (typeof (data) == "string") {
                datan = $.parseJSON(data);
            }
            else {
                datan = data;
            }


            switch (datan.t) {
                case "s": $scope.$broadcast("addOrderList", datan.d); break;
                case "k": $scope.$broadcast("addTitketTask", datan.d); notifyService.getAudio("有新门票任务，请及时处理"); break;
                case "t": $scope.$broadcast("addTaxTask", datan.d); notifyService.getAudio("有新租车任务，请及时处理"); break;
                case "h": $scope.$broadcast("addHotelTask", datan.d); notifyService.getAudio("有酒店任务，请及时处理"); break;
                case "u": $scope.$broadcast("updateTaskState", datan.d); break;
                case "o": $scope.$broadcast("updateOrderState", datan.d); break;
            }

        }
        $connection.setReceived(receiveOrder);

        $scope.adapterSend = function (data) {
            $connection.send(data);
        }

        $scope.createOrder = function () {
            $scope.order.orderDate = new Date();
            $scope.order.customer = $scope.customer;
            OrderService.createOrder($scope.order).success(createSuccess).error(function () {
                $scope.infoAlert("err", "下单失败，请重试！");
            });
        }

        var chageState = function (state) {
            var connectionStatus = "disconnect";
            switch (state.newState) {
                case $.connection.connectionState.connected: connectionStatus = 'Connected'; break;
                case $.connection.connectionState.connecting: connectionStatus = 'Connecting'; break;
                case $.connection.connectionState.reconnecting: connectionStatus = 'Reconnecting'; break;
                case $.connection.connectionState.disconnected: connectionStatus = 'Disconnected'; break;
            }
            $scope.state = connectionStatus;
        }

        $connection.setStateChange(chageState);

        $connection.run();


    }];

    var DisplayTaskCtrl = ["$scope", "task", "state", "$modalInstance", "$TaskInfo", "notifyService", function ($scope, task, state, $modalInstance, $TaskInfo, notifyService) {
        $scope.task = [];
        $scope.task = task;
        $scope.remark = "";
        $scope.step = Number(state) + 1;
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        }
        $scope.checkin = function (id) {
            $TaskInfo.updateTask(id, 1, "领取任务(" + $scope.remark + ")").success(function () {
                //更新任务状态
                $scope.step = 2;
                notifyService.getAudio("领取任务成功");
            });
        }

        $scope.finish = function (id) {
            $TaskInfo.updateTask(id, 2, "完成任务[" + $scope.remark + "]").success(function () {
                notifyService.getAudio("成功完成任务");
                $modalInstance.dismiss('cancel');
            });
        }
        $scope.errOrder = function (id) {
            $TaskInfo.updateTask(id, 4, "下单失败[" + $scope.remark + "]").success(function () {
                notifyService.getAudio("下单失败");

                $modalInstance.dismiss('cancel');
            });
        }
        $scope.cancelin = function (id) {
            $TaskInfo.updateTask(id, 8, "领取退单任务[" + $scope.remark + "]").success(function () {
                //更新任务状态
                $scope.step = 10;
            });

        }

        $scope.canceled = function (id) {
            $TaskInfo.updateTask(id, 9, "取消订单完成[" + $scope.remark + "]").success(function () {
                notifyService.getAudio("取消订单任务完成");
                $modalInstance.dismiss('cancel');
            });
        }
        $scope.cancelerr = function (id) {
            $TaskInfo.updateTask(id, 10, "取消订单失败[" + $scope.remark + "]").success(function () {
                $modalInstance.dismiss('cancel');
            });
        }
    }];

    var OrderHandleCtrl = ["$scope", function ($scope) {
        $scope.currentTab = 1;
        $scope.activeTab = function (tabIndex) {
            $scope.currentTab = tabIndex;
        }
        $scope.$on("changeTab", function (d, index) {
            $scope.currentTab = index;
            $scope.$broadcast("clearEdit");
        });
    }];

    var OrderListCtrl = ["$scope", "OrderService", "notifyService", function ($scope, OrderService, notifyService) {
        $scope.orderList = [];
        $scope.orderListCurrentPage = 1;
        $scope.pageList = [];
        $scope.orderListTotalCount = 0;
        $scope.pageCount = 0;
        $scope.orderPageInfo = { rowCount: "1", pageSize: "8", userId: "d0e1c8f6-4189-435e-8077-063a4c25cca2" };
        var getall = function () {
            OrderService.gerAllOrder($scope.orderPageInfo).success(function (data) {
                $scope.orderListTotalCount = data.count;
                $scope.pageCount = Math.ceil(data.count / $scope.orderPageInfo.pageSize);
                $scope.pageList = [];
                for (var i = 1; i <= $scope.pageCount ; i++) {
                    $scope.pageList.push(i);
                }
                $scope.orderList = data.orderListViewItems;
            }).error(function (data) {
                alert(data);
            });
        }
        getall();

        var add2List = function (d, data) {
            if (typeof (data) == "string") {
                data = $.parseJSON(data);
            }
            $scope.orderList.unshift(data);
            $scope.$apply();
        }

        $scope.$on("addOrderList", add2List);

        $scope.getPageData = function (number) {
            $scope.orderListCurrentPage = number;
            $scope.orderPageInfo.rowCount = number;
            getall();
        }

        $scope.nextPageData = function () {
            if ($scope.orderListCurrentPage != $scope.pageCount)
                $scope.orderListCurrentPage = $scope.orderListCurrentPage + 1;
            $scope.orderPageInfo.rowCount = $scope.orderListCurrentPage;
            getall();
        }

        $scope.prePageData = function () {
            if ($scope.orderListCurrentPage != 1) {
                $scope.orderListCurrentPage = $scope.orderListCurrentPage - 1;
                $scope.orderPageInfo.rowCount = $scope.orderListCurrentPage;
                getall();
            }
        }

        var updateOrderState = function (d, data) {
            var orderIndex = getItemIndex(data.id, "id", $scope.orderList);

            var order = $scope.orderList[orderIndex];
            order.orderStatus = data.state;
            order.remark = data.remark;
            $scope.$apply();
            switch (data.state) {
                case 1: notifyService.getAudio("亲 订单状态已更新"); break;
                case 2: notifyService.getAudio("亲 订单已被领取"); break;
                case 3: notifyService.getAudio("亲 订单已被完成"); break;
                case 6: notifyService.getAudio("亲 订单下单失败，请及时处理"); break;
                case 9: notifyService.getAudio("亲 订单退单成功"); break;
                case 10: notifyService.getAudio("亲 订单退单失败，请及时处理"); break;
            }
        }
        $scope.$on("updateOrderState", updateOrderState);

    }];

    var DisplayOrderCtrl = ["$scope", "id", "$modalInstance", "OrderService", function ($scope, id, $modalInstance, OrderService) {
        $scope.tasks = [];
        $scope.orderItems = [];
        $scope.id = id;

        var loadData = function (id) {
            OrderService.getById(id).success(initData).error(function () {
                alter("订单信息获取失败，请刷新重试");
            });
        }

        var initData = function (d) {
            $scope.orderItems = d.orderItems;
            $scope.tasks = d.tasks;
        }

        loadData(id);

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        }
        $scope.cancelOrder = function () {
            var oi = $scope.id;
            OrderService.cancelOrder(oi).success(function () {
                $modalInstance.dismiss('cancel');
                alert("取消订单成功");
            }).error(function () {
                alert("取消订单失败");
            });
        }

    }];

    var CancelOrderCtrl = ["$scope", "$modal", "OrderService", function ($scope, $modal, OrderService) {
        $scope.dateOptions = {
            "startingday": 1
        };
        $scope.status = {
            startOpened: false,
            endOpened: false
        };

        $scope.orderList = [];
        $scope.orderCode = "";
        $scope.certCode = "";
        $scope.phoneCode = "";
        $scope.orderStartDate = Date.now();
        $scope.orderEndDate = Date.now();
        $scope.startOpen = function ($event) {
            console.log("excute");
            $scope.status.startOpened = true;
        };


        var displayOrderList = function (d) {
            $scope.orderList = [];
            for (var i in d) {
                $scope.orderList.push(d[i]);
            }
        }

        $scope.editOrder = function (id) {
            var modalInstance = $modal.open({
                templateUrl: '/ClientView/View/ChangeOrderItemList.html',  //指向上面创建的视图
                controller: 'ChangeOrderCtrl',// 初始化模态范围
                size: "lg", //大小配置
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });
            modalInstance.result.then(function (processResult) {

            });
        }

        $scope.detailOrder = function (id) {
            var modalInstance = $modal.open({
                templateUrl: '/ClientView/View/DetailOrder.html',  //指向上面创建的视图
                controller: 'DisplayOrderCtrl',// 初始化模态范围
                size: "lg", //大小配置
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });
            modalInstance.result.then(function (processResult) {

            });

        }
        $scope.findOrder = function () {
            var queryModel = { orderCode: $scope.orderCode, phoneCode: $scope.phoneCode, orderStartDate: $scope.orderStartDate, orderEndDate: $scope.orderEndDate, CertCode: $scope.certCode };
            OrderService.getByQuery(queryModel).success(displayOrderList).error(function (msg) {
                alert(msg);
            });
        }
        $scope.endOpen = function ($event) {
            $scope.status.endOpened = true;
        };

    }];

    var ChangeOrderCtrl = ["$scope", "$modalInstance", "id", "OrderService", function ($scope, $modalInstance, id, OrderService) {
        $scope.listScreen = true;
        $scope.identityTypes = [{ name: "身份证", value: 1 }, { name: "护照", value: 2 }, { name: "其他", value: 3 }];
        $scope.item = {};
        $scope.orderId = "";
        $scope.productUserIndex = 0;
        var loadData = function (id) {
            OrderService.getItemsByOrderId(id).success(initData).error(function () {
                alter("订单信息获取失败，请刷新重试");
            });
        }

        var loadItemData = function (id) {
            OrderService.getDetailItemById(id).success(initItemData).error(function () {
                alter("订单信息获取失败，请刷新重试");
            });
        }
        $scope.sminDate = new Date();
        $scope.smaxDate = new Date(2020, 12, 31);

        $scope.dateOptions = {
            "startingday": 1
        };
        $scope.format = "yyyy-MM-dd";
        $scope.status = {
            startOpened: false,
            endOpened: false
        };

        $scope.startOpen = function ($event) {
            $scope.status.startOpened = true;
        };

        $scope.endOpen = function ($event) {
            $scope.status.endOpened = true;
        };
        $scope.changeEndDate = function () {
            $scope.item.endDate = $scope.item.startDate;
        }

        $scope.orderItem = [];
        $scope.item.persons = [];
        $scope.item.product = [];

        var initItemData = function (d) {
            $scope.item = d;
            $scope.item.persons = d.persons;
            $scope.orderId = d.order.id;
        }

        var initData = function (d) {
            $scope.orderItems = d.orderItems;
        }

        loadData(id);

        $scope.editChangeOrder = function (itemId) {
            $scope.listScreen = false;
            loadItemData(itemId);
        }

        $scope.changeOrderItem = function () {
            var changeOrderModel = { orderId: $scope.orderId, orderItem: $scope.item };
            OrderService.modifyOrderItem(changeOrderModel).success(function () {
                $modalInstance.dismiss('cancel');
                alert("修改成功");
            }).error(function () {
                alert("请重新查询后重试！");
            });

        }

        $scope.ok = function () {
            var orderItem = $scope.item.orderItem;
            orderItem.persons = $scope.item.productUsers;
            orderItem.product = $scope.item.product;
            orderItem.describe = $scope.item.product.typeName + "/" + $scope.item.product.name + "/" + orderItem.quantity;
            $modalInstance.close(orderItem);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel'); // 退出
        }

        var getProductTypeName = function (type) {
            var name = "";
            switch (Number(type)) {
                case 0: name = "套餐"; break;

            }
        }

        $scope.addUser = function () {
            $scope.productUserIndex += 1;
            var user = new ProductUserEmpty($scope.productUserIndex);
            $scope.item.persons.push(user);
        }

        $scope.deleteUser = function (index) {
            $scope.item.persons.splice(index, 1);
        }
    }];

    var CreateOrderCtrl = ["$scope", "$modal", "$timeout", "CustomerService", "ProductService", "ComboService", "OrderService", "notifyService", function ($scope, $modal, $timeout, CustomerService, ProductService, ComboService, OrderService, notifyService) {
        $scope.customerInit = {};
        $scope.searchProduct = {};
        $scope.products = [];
        $scope.order = {};
        $scope.order.orderItems = [];
        $scope.customer = {};
        $scope.customerInit.needCreateUser = false;
        $scope.customerInit.infoShow = false;
        $scope.customerInit.infoMsg = "";
        $scope.customerInit.findPersonText = "查询";
        $scope.customerInit.findPersonState = false;
        $scope.customerInit.errShow = false;
        $scope.customerInit.errMsg = "";
        $scope.infoAlert = function (type, msg) {
            if (type == "info") {
                $scope.customerInit.infoMsg = msg;
                $scope.customerInit.infoShow = true;

            }
            if (type == "err") {
                $scope.customerInit.errMsg = msg;
                $scope.customerInit.errShow = true;
            }
            $timeout($scope.closeAlert, 2000);
        }

        $scope.closeAlert = function () {
            $scope.customerInit.infoMsg = "";
            $scope.customerInit.infoShow = false;
            $scope.customerInit.errMsg = "";
            $scope.customerInit.errShow = false;
        }

        $scope.customerInit.levelTypes = [{ name: "普通会员", value: 1 }, { name: "银牌会员", value: 2 }, { name: "金牌会员", value: 3 }];

        $scope.changeFind = function () {
            if ($scope.customer.cPhone && $scope.customer.cPhone.length == 11) {
                $scope.customerInit.findPersonState = true;
            }
        }

        $scope.findPerson = function () {
            if ($scope.customerInit.findPersonState) {
                $scope.customerInit.findPersonText = "查询中……";
                CustomerService.getByPhone($scope.customer.cPhone).success(function (data) {
                    $scope.customer = data;
                    $scope.customerInit.findPersonText = "查询";
                }).error(function (data, state) {
                    if (state === 404) {
                        $scope.customerInit.findPersonText = "无会员";
                        $scope.customerInit.findPersonState = false;
                        $scope.customer.identityType = 1;
                        $scope.customer.level = 1;
                        $scope.customerInit.needCreateUser = true;
                    }
                });
            }
        }

        $scope.createCustomer = function () {
            CustomerService.createCustomer($scope.customer).success(function (data) {
                $scope.customer = data;
                $scope.customerInit.needCreateUser = false;
                $scope.infoAlert("info", "会员已经创建完成，请继续下一步操作！");
            }).error(function (data) {
                $scope.infoAlert("err", "创建会员失败，请刷新后重试！");
            })
        }

        var createSuccess = function (data) {
            $scope.$emit("changeTab", 1);
            $scope.adapterSend(data);
            notifyService.getAudio("亲 订单创建成功");
        }

        $scope.createOrder = function () {
            $scope.order.orderDate = new Date();
            $scope.order.customer = $scope.customer;
            OrderService.createOrder($scope.order).success(createSuccess).error(function () {
                $scope.infoAlert("err", "下单失败，请重试！");
            });
        }

        var clearInfo = function () {
            $scope.searchProduct.type = 1;
            $scope.searchProduct.pid = "";
            $scope.searchProduct.pName = "";
            $scope.products = [];
            $scope.order = {};
            $scope.order.orderItems = [];
            $scope.customer = {};
        }

        $scope.$on("clearEdit", clearInfo);

        $scope.searchProduct.type = 1;
        $scope.searchProduct.pid = "";
        $scope.searchProduct.pName = "";
        $scope.findProduct = function () {
            $scope.products = [];
            console.log($scope.searchProduct.type);
            if ($scope.searchProduct.type == 1) {
                ProductService.getByCodeOrName($scope.searchProduct.pid, $scope.searchProduct.pName)
                .success(function (data) {
                    $scope.products = data;
                })
                .error(function (data, state) {
                    if (state === 404) {
                        $scope.infoAlert("err", "查无此产品，请重新查找");
                    }
                })
            }
            else {
                ComboService.getByCodeOrName($scope.searchProduct.pid, $scope.searchProduct.pName)
              .success(function (data) {
                  $scope.products = data;
              })
              .error(function (data, state) {
                  if (state === 404) {
                      $scope.infoAlert("err", "查无此产品，请重新查找");
                  }
              })
            }
        }

        $scope.item = { customerInfo: "填写订单信息", id: 0 }
        $scope.OpenProductDialog = function (id, size) {
            var pIndex = getItemIndex(id, "id", $scope.products);

            $scope.item.product = $scope.products[pIndex];

            var puser = new ProductUserEmpty(0);
            puser.personName = $scope.customer.cName;
            puser.personPhone = $scope.customer.cPhone;
            puser.personIdentity = $scope.customer.cIdentity;
            puser.credentialType = $scope.customer.identityType;

            $scope.item.productUsers = [];
            $scope.item.productUsers.push(puser);

            var modalInstance = $modal.open({
                templateUrl: '/ClientView/View/AddOrder.html',  //指向上面创建的视图
                controller: 'EditOrderCtrl',// 初始化模态范围
                size: size, //大小配置
                resolve: {
                    item: function () {
                        return $scope.item;
                    },
                    orderEditItem: function () {
                        return null;
                    }
                }
            });
            modalInstance.result.then(function (orderItem) {
                $scope.order.orderItems.push(orderItem);
            });
        }

        $scope.editOrderItem = function (id) {
            var editItemIndex = getItemIndex(id, "oid", $scope.order.orderItems);
            var modalInstanceEdit = $modal.open({
                templateUrl: '/ClientView/View/AddOrder.html',  //指向上面创建的视图
                controller: 'EditOrderCtrl',// 初始化模态范围
                resolve: {
                    item: function () {
                        return $scope.item;
                    },
                    orderEditItem: function () {
                        return angular.copy($scope.order.orderItems[editItemIndex]);
                    }
                }
            });

            modalInstanceEdit.result.then(function (orderItem) {
                var indexItem = getItemIndex(orderItem.id, "oid", $scope.order.orderItems);
                $scope.order.orderItems.splice(Number(indexItem), 1);
                $scope.order.orderItems.push(orderItem);
            });
        }

        $scope.deleteOrderItem = function (id) {
            var indexItem = getItemIndex(id, "oid", $scope.order.orderItems);
            $scope.order.orderItems.splice(Number(indexItem), 1);
        }
    }]

    var EditOrderCtrl = ["$scope", "$modalInstance", "item", "orderEditItem", "CustomerService", function ($scope, $modalInstance, item, orderEditItem, CustomerService) {
        $scope.identityTypes = [{ name: "身份证", value: 1 }, { name: "护照", value: 2 }, { name: "其他", value: 3 }];
        $scope.item = item;
        if (orderEditItem == null) {
            $scope.item.orderItem = { oid: item.product.id, productId: item.product.id, productType: item.product.type, quantity: 1, describe: "", singlePrice: item.product.price, totalPrice: 0, startDate: new Date(), endDate: new Date(), remark: "", certificateNum: "", customerPrice: 0, certificateDate: "" };
            $scope.item.orderItem.customerPrice = item.product.price;
            $scope.productUserIndex = 0;
            var productUserEmpty = new ProductUserEmpty(0);
            $scope.sminDate = new Date();
            $scope.smaxDate = new Date(2020, 12, 31);

            $scope.dateOptions = {
                "startingday": 1
            };
            $scope.format = "yyyy-MM-dd";
            $scope.status = {
                startOpened: false,
                endOpened: false
            };

            $scope.startOpen = function ($event) {
                $scope.status.startOpened = true;
            };

            $scope.endOpen = function ($event) {
                $scope.status.endOpened = true;
            };
            $scope.changeEndDate = function () {
                $scope.item.orderItem.endDate = $scope.item.orderItem.startDate;
            }
        }
        else {
            $scope.item.orderItem = orderEditItem;
            $scope.item.productUsers = orderEditItem.persons;
            $scope.item.product = orderEditItem.product;
        }


        $scope.ok = function () {
            var orderItem = $scope.item.orderItem;
            orderItem.persons = $scope.item.productUsers;
            orderItem.product = $scope.item.product;
            orderItem.describe = $scope.item.product.typeName + "/" + $scope.item.product.name + "/" + orderItem.quantity;
            $modalInstance.close(orderItem);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel'); // 退出
        }

        var getProductTypeName = function (type) {
            var name = "";
            switch (Number(type)) {
                case 0: name = "套餐"; break;

            }


        }
        $scope.addUser = function () {
            $scope.productUserIndex += 1;
            var user = new ProductUserEmpty($scope.productUserIndex);
            $scope.item.productUsers.push(user);
        }
        $scope.deleteUser = function (index) {
            $scope.item.productUsers.splice(index, 1);
        }
    }];

    var TitketCtrl = ["$scope", "$TaskInfo", "taskType", "$modal", function ($scope, $TaskInfo, taskType, $modal) {
        $scope.tasks = [];
        $scope.taskListCurrentPage = 1;
        $scope.pageList = [];
        $scope.taskTotalCount = 0;
        $scope.pageCount = 0;
        $scope.taskQueryModel = { rowCount: 1, pageSize: "8", pt: taskType.Titcket };

        $scope.myTasks = [];
        $scope.taskListMyCurrentPage = 1;
        $scope.pageMyList = [];
        $scope.myTaskTotalCount = 0;
        $scope.myPageCount = 0;
        $scope.myTaskQueryModel = { rowCount: 1, pageSize: "8", pt: taskType.Titcket };

        var getAllMyTask = function () {
            $TaskInfo.getMyTasks($scope.myTaskQueryModel).success(function (data) {
                $scope.myTaskTotalCount = data.count;
                $scope.myPageCount = Math.ceil(data.count / $scope.myTaskQueryModel.pageSize);
                $scope.pageMyList = [];
                for (var i = 1; i <= $scope.myPageCount ; i++) {
                    $scope.pageMyList.push(i);
                }
                $scope.myTasks = data.toms;
            });
        }

        $scope.getMyPageData = function (number) {
            $scope.taskListMyCurrentPage = number;
            $scope.myTaskQueryModel.rowCount = number;
            getAllMyTask();
        }

        $scope.nextMyPageData = function () {
            if ($scope.taskListMyCurrentPage != $scope.myPageCount)
                $scope.taskListMyCurrentPage = $scope.taskListMyCurrentPage + 1;
            $scope.myTaskQueryModel.rowCount = $scope.taskListMyCurrentPage;
            getAllMyTask();
        }

        $scope.preMyPageData = function () {
            if ($scope.taskListMyCurrentPage != 1) {
                $scope.taskListMyCurrentPage = $scope.taskListMyCurrentPage - 1;
                $scope.myTaskQueryModel.rowCount = $scope.taskListMyCurrentPage;
                getAllMyTask();
            }
        }

        var getall = function () {
            $TaskInfo.getTasks($scope.taskQueryModel).success(function (data) {
                $scope.taskTotalCount = data.count;
                $scope.pageCount = Math.ceil(data.count / $scope.taskQueryModel.pageSize);
                $scope.pageList = [];
                for (var i = 1; i <= $scope.pageCount ; i++) {
                    $scope.pageList.push(i);
                }
                $scope.tasks = data.toms;
            });
        }
        getall();

        $scope.getPageData = function (number) {
            $scope.taskListCurrentPage = number;
            $scope.taskQueryModel.rowCount = number;
            getall();
        }

        $scope.nextPageData = function () {
            if ($scope.taskListCurrentPage != $scope.pageCount)
                $scope.taskListCurrentPage = $scope.taskListCurrentPage + 1;
            $scope.taskQueryModel.rowCount = $scope.taskListCurrentPage;
            getall();
        }

        $scope.prePageData = function () {
            if ($scope.taskListCurrentPage != 1) {
                $scope.taskListCurrentPage = $scope.taskListCurrentPage - 1;
                $scope.taskQueryModel.rowCount = $scope.taskListCurrentPage;
                getall();
            }
        }

        $scope.currentTab = 1;
        $scope.activeTab = function (id) {
            if (id == 2) {
                getAllMyTask();
            }
            $scope.currentTab = id;
        }

        var addTitketTask = function (d, data) {
            if (typeof (data) == "string") {
                data = $.parseJSON(data);
            }
            $scope.tasks.unshift(data);
            $scope.$apply();

        }

        var updateTitketTask = function (d, data) {
            var taskIndex = getItemIndex(data.id, "id", $scope.tasks);
            var myTaskIndex = getItemIndex(data.id, "id", $scope.myTasks);

            if (taskIndex !== -1) {
                var task = $scope.tasks[taskIndex];
                task.state = data.state;
                task.remark = data.remark;
            }
            if (myTaskIndex !== -1) {
                var mytask = $scope.myTasks[myTaskIndex];
                mytask.state = data.state;
                mytask.remark = data.remark;
            }
            $scope.$apply();
        }


        $scope.openTaskDialog = function (id, state, isMyTask) {
            var taskIndex = {};
            if (isMyTask) {
                taskIndex = getItemIndex(id, "id", $scope.myTasks);
                $scope.task = $scope.myTasks[taskIndex];
            }
            else {
                taskIndex = getItemIndex(id, "id", $scope.tasks);
                $scope.task = $scope.tasks[taskIndex];
            }


            var modalInstance = $modal.open({
                templateUrl: '/ClientView/View/DetailTask.html',  //指向上面创建的视图
                controller: 'DisplayTaskCtrl',// 初始化模态范围
                size: "lg", //大小配置
                resolve: {
                    task: function () {
                        return $scope.task;
                    },
                    state: function () {
                        return state;
                    }
                }
            });
            modalInstance.result.then(function (responseModel) {

            });
        }

        $scope.$on("addTitketTask", addTitketTask);
        $scope.$on("updateTaskState", updateTitketTask);
    }];

    var TaxCtrl = ["$scope", "$TaskInfo", "taskType", "$modal", function ($scope, $TaskInfo, taskType, $modal) {
        $scope.tasks = [];
        $scope.taskListCurrentPage = 1;
        $scope.pageList = [];
        $scope.taskTotalCount = 0;
        $scope.pageCount = 0;
        $scope.taskQueryModel = { rowCount: 1, pageSize: "8", pt: taskType.Tax };

        $scope.myTasks = [];
        $scope.taskListMyCurrentPage = 1;
        $scope.pageMyList = [];
        $scope.myTaskTotalCount = 0;
        $scope.myPageCount = 0;
        $scope.myTaskQueryModel = { rowCount: 1, pageSize: "8", pt: taskType.Tax };

        var getAllMyTask = function () {

            $TaskInfo.getMyTasks($scope.myTaskQueryModel).success(function (data) {
                $scope.myTaskTotalCount = data.count;
                $scope.myPageCount = Math.ceil(data.count / $scope.myTaskQueryModel.pageSize);
                $scope.pageMyList = [];
                for (var i = 1; i <= $scope.myPageCount ; i++) {
                    $scope.pageMyList.push(i);
                }
                $scope.myTasks = data.toms;
            });
        }

        $scope.getMyPageData = function (number) {
            $scope.taskListMyCurrentPage = number;
            $scope.myTaskQueryModel.rowCount = number;
            getAllMyTask();
        }

        $scope.nextMyPageData = function () {
            if ($scope.taskListMyCurrentPage != $scope.myPageCount)
                $scope.taskListMyCurrentPage = $scope.taskListMyCurrentPage + 1;
            $scope.myTaskQueryModel.rowCount = $scope.taskListMyCurrentPage;
            getAllMyTask();
        }

        $scope.preMyPageData = function () {
            if ($scope.taskListMyCurrentPage != 1) {
                $scope.taskListMyCurrentPage = $scope.taskListMyCurrentPage - 1;
                $scope.myTaskQueryModel.rowCount = $scope.taskListMyCurrentPage;
                getAllMyTask();
            }
        }

        var getall = function () {
            $TaskInfo.getTasks($scope.taskQueryModel).success(function (data) {
                $scope.taskTotalCount = data.count;
                $scope.pageCount = Math.ceil(data.count / $scope.taskQueryModel.pageSize);
                $scope.pageList = [];
                for (var i = 1; i <= $scope.pageCount ; i++) {
                    $scope.pageList.push(i);
                }
                $scope.tasks = data.toms;
            });
        }

        getall();

        $scope.getPageData = function (number) {
            $scope.taskListCurrentPage = number;
            $scope.taskQueryModel.rowCount = number;
            getall();
        }

        $scope.nextPageData = function () {
            if ($scope.taskListCurrentPage != $scope.pageCount)
                $scope.taskListCurrentPage = $scope.taskListCurrentPage + 1;
            $scope.taskQueryModel.rowCount = $scope.taskListCurrentPage;
            getall();
        }

        $scope.prePageData = function () {
            if ($scope.taskListCurrentPage != 1) {
                $scope.taskListCurrentPage = $scope.taskListCurrentPage - 1;
                $scope.taskQueryModel.rowCount = $scope.taskListCurrentPage;
                getall();
            }
        }


        $scope.currentTab = 1;
        $scope.activeTab = function (id) {
            getAllMyTask();

            $scope.currentTab = id;
        }

        $scope.openTaskDialog = function (id, state, isMyTask) {
            var taskIndex = {};
            if (isMyTask) {
                taskIndex = getItemIndex(id, "id", $scope.myTasks);
                $scope.task = $scope.myTasks[taskIndex];
            }
            else {
                taskIndex = getItemIndex(id, "id", $scope.tasks);
                $scope.task = $scope.tasks[taskIndex];
            }
            var modalInstance = $modal.open({
                templateUrl: '/ClientView/View/DetailTask.html',  //指向上面创建的视图
                controller: 'DisplayTaskCtrl',// 初始化模态范围
                size: "lg", //大小配置
                resolve: {
                    task: function () {
                        return $scope.task;
                    },
                    state: function () {
                        return state;
                    }
                }
            });
            modalInstance.result.then(function (responseModel) {

            });
        }

        var addTaxTask = function (d, data) {
            if (typeof (data) == "string") {
                data = $.parseJSON(data);
            }
            $scope.tasks.unshift(data);
        }

        var updateTaxTask = function (d, data) {
            var taskIndex = getItemIndex(data.id, "id", $scope.tasks);
            var myTaskIndex = getItemIndex(data.id, "id", $scope.myTasks);

            if (taskIndex !== -1) {
                var task = $scope.tasks[taskIndex];
                task.state = data.state;
                task.remark = data.remark;
            }
            if (myTaskIndex !== -1) {
                var mytask = $scope.myTasks[myTaskIndex];
                mytask.state = data.state;
                mytask.remark = data.remark;
            }
        }

        $scope.$on("addTaxTask", addTaxTask);
        $scope.$on("updateTaskState", updateTaxTask);
    }];

    var HotelCtrl = ["$scope", "$TaskInfo", "taskType", "$modal", function ($scope, $TaskInfo, taskType, $modal) {
        $scope.tasks = [];
        $scope.taskListCurrentPage = 1;
        $scope.pageList = [];
        $scope.taskTotalCount = 0;
        $scope.pageCount = 0;
        $scope.taskQueryModel = { rowCount: 1, pageSize: "8", pt: taskType.Hotel };

        $scope.myTasks = [];
        $scope.taskListMyCurrentPage = 1;
        $scope.pageMyList = [];
        $scope.myTaskTotalCount = 0;
        $scope.myPageCount = 0;
        $scope.myTaskQueryModel = { rowCount: 1, pageSize: "8", pt: taskType.Hotel };

        var getAllMyTask = function () {

            $TaskInfo.getMyTasks($scope.myTaskQueryModel).success(function (data) {
                $scope.myTaskTotalCount = data.count;
                $scope.myPageCount = Math.ceil(data.count / $scope.myTaskQueryModel.pageSize);
                $scope.pageMyList = [];
                for (var i = 1; i <= $scope.myPageCount ; i++) {
                    $scope.pageMyList.push(i);
                }
                $scope.myTasks = data.toms;
            });
        }

        $scope.getMyPageData = function (number) {
            $scope.taskListMyCurrentPage = number;
            $scope.myTaskQueryModel.rowCount = number;
            getAllMyTask();
        }

        $scope.nextMyPageData = function () {
            if ($scope.taskListMyCurrentPage != $scope.myPageCount)
                $scope.taskListMyCurrentPage = $scope.taskListMyCurrentPage + 1;
            $scope.myTaskQueryModel.rowCount = $scope.taskListMyCurrentPage;
            getAllMyTask();
        }

        $scope.preMyPageData = function () {
            if ($scope.taskListMyCurrentPage != 1) {
                $scope.taskListMyCurrentPage = $scope.taskListMyCurrentPage - 1;
                $scope.myTaskQueryModel.rowCount = $scope.taskListMyCurrentPage;
                getAllMyTask();
            }
        }

        var getall = function () {
            $TaskInfo.getTasks($scope.taskQueryModel).success(function (data) {
                $scope.taskTotalCount = data.count;
                $scope.pageCount = Math.ceil(data.count / $scope.taskQueryModel.pageSize);
                $scope.pageList = [];
                for (var i = 1; i <= $scope.pageCount ; i++) {
                    $scope.pageList.push(i);
                }
                $scope.tasks = data.toms;
            });
        }

        getall();

        $scope.getPageData = function (number) {
            $scope.taskListCurrentPage = number;
            $scope.taskQueryModel.rowCount = number;
            getall();
        }

        $scope.nextPageData = function () {
            if ($scope.taskListCurrentPage != $scope.pageCount)
                $scope.taskListCurrentPage = $scope.taskListCurrentPage + 1;
            $scope.taskQueryModel.rowCount = $scope.taskListCurrentPage;
            getall();
        }

        $scope.prePageData = function () {
            if ($scope.taskListCurrentPage != 1) {
                $scope.taskListCurrentPage = $scope.taskListCurrentPage - 1;
                $scope.taskQueryModel.rowCount = $scope.taskListCurrentPage;
                getall();
            }
        }

        $scope.currentTab = 1;
        $scope.activeTab = function (id) {
            getAllMyTask();
            $scope.currentTab = id;
        }
        $scope.openTaskDialog = function (id, state, isMyTask) {
            var taskIndex = {};
            if (isMyTask) {
                taskIndex = getItemIndex(id, "id", $scope.myTasks);
                $scope.task = $scope.myTasks[taskIndex];
            }
            else {
                taskIndex = getItemIndex(id, "id", $scope.tasks);
                $scope.task = $scope.tasks[taskIndex];
            }
            var modalInstance = $modal.open({
                templateUrl: '/ClientView/View/DetailTask.html',  //指向上面创建的视图
                controller: 'DisplayTaskCtrl',// 初始化模态范围
                size: "lg", //大小配置
                resolve: {
                    task: function () {
                        return $scope.task;
                    },
                    state: function () {
                        return state;
                    }
                }
            });
            modalInstance.result.then(function (responseModel) {

            });
        }
        var addHotelTask = function (d, data) {
            if (typeof (data) == "string") {
                data = $.parseJSON(data);
            }
            $scope.tasks.unshift(data);
        }

        var updateHotelTask = function (d, data) {
            var taskIndex = getItemIndex(data.id, "id", $scope.tasks);
            var myTaskIndex = getItemIndex(data.id, "id", $scope.myTasks);

            if (taskIndex !== -1) {
                var task = $scope.tasks[taskIndex];
                task.state = data.state;
                task.remark = data.remark;
            }
            if (myTaskIndex !== -1) {
                var mytask = $scope.myTasks[myTaskIndex];
                mytask.state = data.state;
                mytask.remark = data.remark;
            }
        }

        $scope.$on("addHotelTask", addHotelTask);
        $scope.$on("updateTaskState", updateHotelTask);
    }];

    var ChartCtrl = ["$scope", "$hub", function ($scope, $hub) {
        $scope.currentTab = 1;
        $scope.activeTab = function (id) {
            $scope.currentTab = id;
        }

        $scope.phone = "";
        $scope.message = "";
        $scope.messageResults = [];

        var pushMsg = function (msg) {
            var msgModel = new Msg();
            msgModel.id = $scope.messageResults.length;
            msgModel.createDate = new Date();
            msgModel.from = msg.phone;
            msgModel.to = "other";
            msgModel.content = msg.context;
            $scope.messageResults.push(msgModel);
            $scope.$apply();
        }

        $hub.setReceived(pushMsg);

        $scope.sendMessge = function () {
            var sm = { phone: $scope.phone, context: $scope.message };
            $scope.send(sm);
        }

        $scope.send = $hub.getSend;
        $hub.run();
    }];

    app.controller("UIRouterCtrl", UIRouterCtrl);
    app.controller("ChartCtrl", ChartCtrl);
    app.controller("OrderListCtrl", OrderListCtrl);
    app.controller("CreateOrderCtrl", CreateOrderCtrl);
    app.controller("EditOrderCtrl", EditOrderCtrl);
    app.controller("CancelOrderCtrl", CancelOrderCtrl);
    app.controller("OrderHandleCtrl", OrderHandleCtrl);
    app.controller("ChangeOrderCtrl", ChangeOrderCtrl);
    app.controller("TaskAdapterCtrl", TaskAdapterCtrl);
    app.controller("DisplayTaskCtrl", DisplayTaskCtrl);
    app.controller("DisplayOrderCtrl", DisplayOrderCtrl);
    app.controller("TitketCtrl", TitketCtrl);
    app.controller("TaxCtrl", TaxCtrl);
    app.controller("HotelCtrl", HotelCtrl);
})(angular.module("orderSys"), jQuery)