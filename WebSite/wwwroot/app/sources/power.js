angular.module("homeIot.power", [])
    .component('power', {
        templateUrl: '/views/power.html',
        controller: 'PowerController',
        controllerAs: 'power'
    })
.controller('PowerController', PowerController);

function PowerController() {

};