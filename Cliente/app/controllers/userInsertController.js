app.controller("userInsertController",
    function ($scope, $location, userService, authService) {
        $scope.needLogin = authService.getNeedLogin();

        $scope.registrar = function (validModel) {
            if (validModel) {
                $scope.doRegister = true;
                $scope.errorSistema = false;

                var handleInsert = function (res) {
                    if (res.status == 201) { //Insertado
                        if ($scope.needLogin) {
                            var handleLog = function (res) {
                                $scope.doRegister = false;
                                if (res.status == 200) {
                                    $location.path("/usuarios");
                                } else {
                                    $location.path("/");
                                }
                            }
                            //Registro nuevo sin usuario autenticado, necesita hacer login
                            userService.login($scope.usuario.NombreUsuario, $scope.usuario.Contrasena)
                                .then(handleLog, handleLog);
                        } else {
                            $location.path("/usuarios");
                        }
                    } else {
                        $scope.errores = res.data;
                        $scope.doRegister = false;
                    }
                };

                userService.registrar($scope.usuario)
                    .then(handleInsert, handleInsert);
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
    }
);