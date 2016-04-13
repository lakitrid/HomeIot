angular.module("homeIot.admin", [])
    .component('admin', {
        templateUrl: '/views/admin.html',
        controller: 'AdminController',
        controllerAs: 'admin',
        $routeConfig: [
            { path: '/users/...', name: 'Users', component: 'users', useAsDefault: true }
        ]
    })
.controller('AdminController', AdminController);

function AdminController() {

};