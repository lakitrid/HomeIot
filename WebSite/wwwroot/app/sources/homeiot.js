angular.module("homeIot", [
        "ngComponentRouter",
        "homeIot.home",
        "homeIot.dashboard",
        "homeIot.settings",
        "homeIot.power"
])
    .config([function () {
    }])
    .value('$routerRootComponent', 'home');