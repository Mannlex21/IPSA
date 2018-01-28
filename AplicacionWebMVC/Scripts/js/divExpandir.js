$(function() {
  $("a.tabs").click(function(e) {
    var target = $(this).data("target");
    if (target == "#fm1") {
        var isTargetExpanded = $(target).hasClass('in');
        if (isTargetExpanded) {
            $('#abrirCerrar').removeClass('glyphicon glyphicon-minus').addClass('glyphicon glyphicon-plus');
        } else {
            $('#abrirCerrar').removeClass('glyphicon glyphicon-plus').addClass('glyphicon glyphicon-minus');
        }
    } else if (target == "#fm2") {
        var isTargetExpanded = $(target).hasClass('in');
        if (isTargetExpanded) {
            $('#abrirCerrar2').removeClass('glyphicon glyphicon-minus').addClass('glyphicon glyphicon-plus');
        } else {
            $('#abrirCerrar2').removeClass('glyphicon glyphicon-plus').addClass('glyphicon glyphicon-minus');
        }
    }
    
  });
});