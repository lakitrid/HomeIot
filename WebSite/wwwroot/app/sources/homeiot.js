angular.module("homeIot", [
        "ngComponentRouter",
        "homeIot.home",
        "homeIot.dashboard",
        "homeIot.detail"
])
    .config([function () {
    }])
    .value('$routerRootComponent', 'home');