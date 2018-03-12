$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});
function Cotizacion(preRequisicion, departamento, ejercicio,uso,ciclo) {
    $('#modalCotizacion').modal('show');
    $("#modalCotizacion").on('shown.bs.modal', function () {
        //$('#gridDet').jqGrid('setGridWidth', $(".modalB").width());
        $(".loaderC").fadeOut(500, function () {
            $("#divC").fadeIn(500);
            console.log("entro")
            $("#gridDet").jqGrid().setGridParam({ postData: { preRequisicion: preRequisicion, ejercicio: ejercicio, departamento: departamento }, page: 1 }).trigger('reloadGrid');
        });
    }); 
    
}