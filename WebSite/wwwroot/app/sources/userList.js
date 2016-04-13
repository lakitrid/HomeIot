angular.module("homeIot.userList", [])
    .component('userList', {
        templateUrl: '/views/userList.html',
        controller: 'userListController',
        controllerAs: 'userList'
    })
.controller('userListController', userListController);

function userListController($http) {
    var self = this;
    this.users = [];

    $http.get('/services/user').then(function (success) {
        self.users = success.data;
    });
};

userListController.$inject = ['$http'];