app.controller("indexController",
    function ($scope, $location, authService) {

        $scope.isAuthed = authService.isAuthed;

        $scope.logout = function () {
            authService.logout();
            $location.path("/");
        }
    }
);