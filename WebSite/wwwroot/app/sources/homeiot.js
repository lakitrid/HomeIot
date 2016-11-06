angular.module("homeIot", [
        "ngComponentRouter",
        "homeIot.home",
        "homeIot.dashboard",
        "homeIot.settings",
        "homeIot.power",
        "homeIot.admin",
        "homeIot.users",
        "homeIot.userList",
        "homeIot.useredit"
])
    .config([function () {
    }])
    .value('$routerRootComponent', 'home');