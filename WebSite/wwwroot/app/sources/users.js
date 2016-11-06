angular.module("homeIot.users", [])
    .component('users', {
        templateUrl: '/views/users.html',
        controller: 'usersController',
        controllerAs: 'users',
        $routeConfig: [
            { path: '/', name: 'UserList', component: 'userList', useAsDefault: true },
            { path: '/:login', name: 'UserEdit', component: 'userEdit' }
        ]
    })
.controller('usersController', usersController);

function usersController() {
    var self = this;
};