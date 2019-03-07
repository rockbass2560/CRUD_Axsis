app.factory("userFactory", function () {
    return {
        getDefaultUser: function () {
            return {
                nombreUsuario: "",
                contrasena: ""
            }
        },
        getEmptyUser: function (id) {
            return {
                "id" : id,
                "nombreUsuario" : "",
                "contrasena" : "",
                "sexo" : ""
            }
        }
    }
});