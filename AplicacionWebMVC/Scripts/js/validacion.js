var prerequisicionG;
var chkBox;
$(function () {
    // Initialize form validation on the registration form.
    // It has the name attribute "registration"
    
    $(document).on("click", "#btnGuardar", function () {
        reestablecerInputsError();
        if ($.isEmptyObject(obj)) {
            $("#errTablaP").text("*Se necesita al menos una partida");
        } else {
            $("#errTablaP").text("");
        }
        $("form[name='infoSol']").validate({
            rules: {
                departamento: "required",
                uso: "required",
                fechaNecesitar: "required",
                fechaActual: "required",
                ejercicio: {required:true,number:true} ,
                partidaPrep: "required"
            },
            errorPlacement: function (error, element) {
                //Custom position: first name
                if (element.attr("name") == "departamento") {
                    $("#errDepartamento").text("*Es requerido");
                }
                if (element.attr("name") == "uso") {
                    $("#errUso").text("*Es requerido");
                }
                if (element.attr("name") == "fechaNecesitar") {
                    $("#errFechaN").text("*Es requerido");
                }
                if (element.attr("name") == "fechaActual") {
                    $("#errFechaA").text("*Es requerido");
                }
                if (element.attr("name") == "ejercicio") {
                    if (error.text().indexOf("number") != -1) {
                        $("#errEjercicio").text("*Debe ser año en numero");
                    }
                    if (error.text().indexOf("required") != -1) {
                        $("#errEjercicio").text("*Es requerido");
                    }
                }
                if (element.attr("name") == "partidaPrep") {
                    $("#errPartidaPrep").text("*Es requerido");
                }
                else {
                    error.append($('.errorTxt span'));
                }
            },
            messages: {
                
            }
        });

        if ($("form[name='infoSol']").valid()) {
            if (!$.isEmptyObject(obj)) {
                guardarSolicitud()
            } else{
                $("#errTablaP").text("*Se necesita al menos una partida");
            }
        } 
    });
    function guardarSolicitud() {
        
        var obs = document.getElementById("observaciones").value;
        var data = {
            idRequisicion: "n/a",
            fechaRequisicion: document.getElementById("fechaActual").value,
            uso: document.getElementById("uso").value,
            fechaNecesitar: document.getElementById("fechaNecesitar").value,
            departamento: document.getElementById("departamentoD").value,
            ciclo: document.getElementById("ciclo").value,
            area: document.getElementById("departamentoA").value,
            fechaRecepcion: "01/01/2017",
            observaciones:obs,
            ejercicio: document.getElementById("ejercicio").value,
            partidaPresupuestal: document.getElementById("partidaPrep").value,
            solicitante: 0,
        };
        chkBox= {
            trabajoSindicato: $("#chkB1").is(':checked'),
            retencionImpuesto: $("#chkB2").is(':checked'),
            altura: $("#chkB3").is(':checked'),
            espaciosConfinados: $("#chkB4").is(':checked'),
            electrico: $("#chkB5").is(':checked'),
            corte: $("#chkB6").is(':checked'),
            soldadura: $("#chkB7").is(':checked'),
            izajes: $("#chkB8").is(':checked'),
            montacarga: $("#chkB9").is(':checked'),
        };
        var d = {
            solicitud: data,
            partidas: obj,
            checks: chkBox,
            username: username
        }
        var url = $('#requisicionURL').data('request-url');
        var bandera = 0;
        if ($("#file1")[0].files.length != 0) {
            if ($("#file1")[0].files[0].size > 150000000) {
                bandera = 1;
                swal("Error!", "Ocurrio el siguiente error: El archivo excede el limite permitido. Limite: 150 mb ", "error");
            } else {
                bandera = 0;
            }
        } 
        if (bandera===0) {
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(d),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cache: false,
                beforeSend: function () {
                    $(".loader").fadeIn(1);
                },
                complete: function () {

                }, error: function (xhr, textStatus, exceptionThrown) {
                    var errorData = $.parseJSON(xhr.responseText);
                    $(".loader").fadeOut(100);
                    swal("Error!", "Ocurrio el siguiente error: " + errorData[0], "error");
                },
                success: function (result) {
                    var files = $("#file1");
                    document.getElementById("btnImprimir").disabled = false;
                    prerequisicionG = result.preRequisicion;
                    var totalFiles = files[0].files.length;
                    var data = new FormData();
                    data.append("id", result.preRequisicion);
                    data.append("dep", document.getElementById("departamentoD").value);
                    data.append("ejercicio", document.getElementById("ejercicio").value);
                    for (var i = 0; i < totalFiles; i++) {
                        data.append("Foto", files[0].files[i]);
                    }
                    var fileUploadUrl = $('#UploadFileURL').data('request-url');
                    $.ajax({
                        url: fileUploadUrl,
                        data: data,
                        type: 'POST',
                        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                        processData: false, // NEEDED
                        success: function (response) {
                            document.getElementById("file1").value = "";
                            var d = {
                                Subject: "Se agrego una pre-requisicion",
                                fechaNecesitar: document.getElementById("fechaNecesitar").value,
                                fechaRequisicion: document.getElementById("fechaActual").value,
                                uso: document.getElementById("uso").value,
                                observaciones: document.getElementById("observaciones").value,
                                departamento: document.getElementById("departamento").value
                            };
                            var url = $('#emailURL').data('request-url');
                            $.ajax({
                                type: "POST",
                                url: url,
                                data: JSON.stringify(d),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (result) {
                                    $(".loader").fadeOut(100);
                                    swal("Guardado!", "Se ha guardado la solicitud correctamente", "success");
                                    console.log("Se envio mail");
                                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    $(".loader").fadeOut(100);
                                    swal("Aviso!", "Se ha guardado la solicitud, pero no se ha enviado el correo. Error:" + errorThrown, "warning");
                                    console.log("No se envio mail");
                                }
                            });
                        },
                        error: function (error) {
                            console.log(error);
                            $(".loader").fadeOut(100);
                            swal("Aviso!", "Se ha guardado la solicitud, pero no se pudieron subir los adjuntos. Error: " + error.statusText, "warning");
                        }
                    });
                }
            });
        }
        
       
    }
});
function reestablecerInputsError() {
    $("#errDepartamento").text("");
    $("#errFechaN").text("");
    $("#errUso").text("");
    $("#errFechaA").text("");
    $("#errEjercicio").text("");
    $("#errPartidaPrep").text("");
    $("#errObservaciones").text("");
    $("#errDescripcion").text("");
    $("#errCantidad").text("");
    $("#errUnidad").text("");
    $("#errExistencia").text("");
    $("#errClave").text("");
    $("#errPrecioU").text("");
    $("#errDetalle").text("");
    $("#errTablaP").text("");
}
function removeClass(node, cls) {
    if (node && node.className && node.className.indexOf(cls) >= 0) {
        var pattern = new RegExp('\\s*' + cls + '\\s*');
        node.className = node.className.replace(pattern, ' ');
    }
}