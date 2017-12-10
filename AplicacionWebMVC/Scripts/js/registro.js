$(function () {
    if ($('#Role').val() =="Revisor2") {
        $("#divOpciones").show();
    } else {
        $("#divOpciones").hide();
    }
    $("#Role").change(function (ev) {
        console.log(ev.target.value);
        if (ev.target.value=="Revisor2") {
            console.log("entro")
            $("#divOpciones").show();
        } else {
            console.log("salio")
            $("#divOpciones").hide();
        }
    });
    $('#guardarRegistro').click(function (e) {
    });
});