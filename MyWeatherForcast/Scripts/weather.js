$(document).ready(function() {
        $("#btnSubmit").click(function () {
            if ($("#Location").val() == "" && $("#NumberOfDays").val() == "") {
                alert("There is no value in textbox");
            }
        });
    
    var dayweather = document.getElementsByClassName("day-weather");
    var count = 1;
    if (dayweather) {
        for (i = 1; i <= dayweather.length; i++) {
            var infoCont = "wInfo" + count.toString();
            var wInfo = document.getElementById(infoCont);
            var bgclass = wInfo.className.replace(' ', '');
            wInfo.closest('div.day-weather').classList.add(bgclass);
            count++;
        }
    }
    
});
//function classSetter() {
    
//}
//window.onload = function () {
//    setTimeout(classSetter, 5000);
//};