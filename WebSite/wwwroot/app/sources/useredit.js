angular.module("homeIot.useredit", [])
    .component('userEdit', {
        templateUrl: '/views/userEdit.html',
        controller: 'userEditController',
        controllerAs: 'userEdit'
    })
.controller('userEditController', userEditController);

function userEditController($http, $scope) {
    var self = this;

    self.user = {};
    self.errors = [];

    self.newUser = true;

    self.invalidConfirm = false;

    this.$routerOnActivate = function (next) {
        if (angular.isDefined(next.params.login)) {
            self.newUser = false;

            $http.get('/services/user/' + next.params.login).then(function (success) {
                self.user = success.data;
            });
        }
    };

    self.edit = function () {
        // Check if the form is valid
        if ($scope.userForm.$valid) {
            // Check if we have the same password when we are creating a new user
            if (!self.newUser || self.user.Password === self.user.ConfirmPassword) {
                $http.post('/services/user', self.user).then(function (success) {
                    this.$router.navigate(['UserList']);
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