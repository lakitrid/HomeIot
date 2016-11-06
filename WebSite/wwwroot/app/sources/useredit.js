angular.module("homeIot.useredit", [])
    .component('userEdit', {
        templateUrl: '/views/userEdit.html',
        controller: 'userEditController',
        controllerAs: 'userEdit',
        bindings: { $router: '<' }
    })
.controller('userEditController', userEditController);

function userEditController($http, $scope) {
    var self = this;

    self.user = {};
    self.routeLogin = undefined;
    self.errors = [];

    self.newUser = true;

    self.invalidConfirm = false;

    this.$routerOnActivate = function (next) {
        if (angular.isDefined(next.params.login)) {
            self.newUser = false;
            self.routeLogin = next.params.login;

            $http.get('/services/user/' + self.routeLogin).then(function (success) {
                self.user = success.data;
            });
        }
    };

    self.edit = function () {
        // Check if the form is valid
        if ($scope.userForm.$valid) {
            // Check if we have the same password when we are creating a new user
            if (self.newUser && self.user.Password === self.user.ConfirmPassword) {
                $http.post('/services/user', self.user).then(function (success) {
                    self.$router.navigate(['UserList']);
                }, function (error) {
                    self.errors = error.data;
                });
            } else if (!self.newUser) {
                $http.put('/services/user/' + self.routeLogin, self.user).then(function (success) {
                    self.$router.navigate(['UserList']);
                }, function (error) {
                    self.errors = error.data;
                });
            } else {
                self.invalidConfirm = true;
            }
        }
    }
};


userEditController.$inject = ['$http', '$scope'];