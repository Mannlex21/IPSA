$(function() {
  $("a.tabs").click(function(e) {
    var target = $(this).data("target");
    var isTargetExpanded = $(target).hasClass('in');
   // $('.show-in-md').addClass('collapse').removeClass('in'); // collapse all before expading clicked target

    if (isTargetExpanded) {
        console.log("entor")
        $('#abrirCerrar').removeClass('glyphicon glyphicon-minus').addClass('glyphicon glyphicon-plus');
        
      //$(target).addClass('collapse').removeClass("in"); // collapse clicked target
    } else {
        $('#abrirCerrar').removeClass('glyphicon glyphicon-plus').addClass('glyphicon glyphicon-minus');
      //$(target).removeClass('collapse').addClass("in"); // expand clicked target
    }
  });
});