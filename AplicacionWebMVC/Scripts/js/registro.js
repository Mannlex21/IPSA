
$(function () {
    if ($('#Role').val() ==="Revisor2") {
        $("#divOpciones").show();
    } else {
        $("#divOpciones").hide();
    }
    $("#Role").change(function (ev) {
        if (ev.target.value==="Revisor2") {
            $("#divOpciones").show();
        } else {
            console.log("salio")
            $("#divOpciones").hide();
        }
    });
    $("form").submit(function (event) {
        //event.preventDefault();
        //window.location = "Home/ListaUsuarios.cshtml";
        //var $ = jQuery;
        $(this).validate();
        if ($(this).valid()) {
            console.log("Entro");

            $(':submit', this).attr('disabled', 'disabled');
        }
        var depa = document.getElementById("departamento");
        var role = document.getElementById("Role");
        var radio;
        if (role.options[role.selectedIndex].value === "Revisor2") {
            radio = $('input[name=tipo]:checked', '#myForm').val();
        } else {
            radio = "n/a";
        }
        var objUsuario = {
            nombreUsuario: document.getElementById('nombreUsuario').value,
            pass: document.getElementById('contrasena').value,
            idEmpleado: document.getElementById('idEmpleado').value,
            registradoPor: document.getElementById('registradoPor').value,
            departamento: depa.options[depa.selectedIndex].value,
            Role: role.options[role.selectedIndex].value,
            tipo: radio
        };
        var d = {
            usuario: objUsuario,
            departamentos: objDepCargo
        }
        console.log(d);
        var url = $('#registroURL').data('request-url');
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(d),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            beforeSend: function () {
            },
            complete: function () {
            }, error: function () {
            },
            success: function () {
                window.location.href = "/Home/ListaUsuarios";
            }
        });
    });
    $(document).on('click', '#guardarRegistro', function (e) {
        
        var depa = document.getElementById("departamento");
        var role = document.getElementById("Role");
        var radio;
        if (role.options[role.selectedIndex].value === "Revisor2") {
            radio = $('input[name=tipo]:checked', '#myForm').val();
        } else {
            radio = "n/a";
        }
        var objUsuario = {
            nombreUsuario: document.getElementById('nombreUsuario').value,
            pass: document.getElementById('contrasena').value,
            idEmpleado: document.getElementById('idEmpleado').value,
            registradoPor: document.getElementById('registradoPor').value,
            departamento: depa.options[depa.selectedIndex].value,
            Role: role.options[role.selectedIndex].value,
            tipo: radio
        };
        if (tieneDato("T") === false) {
            if (tieneDato(depa.options[depa.selectedIndex].value) === false) {
                var element = {
                    "id": depa.options[depa.selectedIndex].value,
                    "text": depa.options[depa.selectedIndex].innerHtml
                };
                objDepCargo.push(element);
            }
        }
        var d = {
            usuario: objUsuario,
            departamentos: objDepCargo
        }
        if (objUsuario.nombreUsuario === "" || objUsuario.pass === null || objUsuario.idEmpleado === null ||
            objUsuario.registradoPor === null || objUsuario.departamento === null || objUsuario.Role === null || objDepCargo.length === 0) {
            swal("Error!", "Se deben de llenar todos los campos", "error");
        } else {
            var url = $('#registroURL').data('request-url');
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(d),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cache: false,
                beforeSend: function () {
                },
                complete: function () {
                }, error: function (result) {
                    console.log(result.message)
                },
                success: function (result) {
                    if (result.code === "Error") {
                        swal("Error!", "Ocurrio el siguiente error: " + result.message, "error");
                    } else {
                        swal("Guardado!", "Se ha registrado el usuario exitosamente", "success").then((value) => {
                            window.location.href = "/Home/ListaUsuarios";
                        });
                    }
                }
            });
        }
    });
    function tieneDato(valor) {
        var c = 0;
        Object.keys(objDepCargo).forEach(function (key) {
            if (objDepCargo[key].id === valor) {
                c++;
            }
        });
        if (c !== 0) {
            return true;
        } else {
            return false;
        }
    }
});