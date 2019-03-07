app.controller("userEditController", function ($scope, $location, userService) {
    $scope.usuario = userService.getUsuario();

    $scope.update = function (validModel) {
        if (validModel) {
            $scope.doUpdate = true;
            $scope.errorSistema = false;

            var handleRequest = function (res) {
                $scope.doUpdate = false;
                if (res.status == 201) { //Actualizado
                    $location.path("/usuarios");
                } else if (res.status == 400) {
                    
                } else {
                    $scope.errores = res.data;
                    $scope.errorSistema = true;
                }
            };

            userService.actualizar($scope.usuario)
                .then(handleRequest, handleRequest);
        }
    };

    $scope.changeText = function (editForm) {
        editForm.contrasenaRepetida.$setValidity("mismaContrasena",
            $scope.usuario.Contrasena === $scope.usuario.ContrasenaRepetida);
    }

    $scope.$watch("usuario.ContrasenaRepetida",
        function (newVal, oldVal) {
           
        }
    );
});