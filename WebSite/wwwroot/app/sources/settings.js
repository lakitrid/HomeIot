angular.module("homeIot.settings", [])
    .component('settings', {
        templateUrl: '/views/settings.html',
        controller: 'SettingsController',
        controllerAs: 'settings'
    })
.controller('SettingsController', SettingsController);

function SettingsController() {

};