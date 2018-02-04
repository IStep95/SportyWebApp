const DEFAULT_SIDEBAR_HEIGHT = '1000px';
const DEFAULT_COL_LG_12_HEIGHT = '900px'; 


document.addEventListener("DOMContentLoaded", function () { windowLoaded() });
function windowLoaded() {
   
    var sideBarWrapperHeight;
    if (document.getElementById("page-content-wrapper") === null)
    {
        document.getElementById("sidebar-wrapper").style.height = DEFAULT_SIDEBAR_HEIGHT;
    }
    else
    {
        sideBarWrapperHeight = $("#page-content-wrapper").css("height");    
        var sideBarWrapperHeightInt = parseInt(sideBarWrapperHeight, 10);
        var DEFAULT_SIDEBAR_HEIGHT_INT = parseInt(DEFAULT_SIDEBAR_HEIGHT, 10);

        if (sideBarWrapperHeightInt < DEFAULT_SIDEBAR_HEIGHT_INT)
        {
            sideBarWrapperHeight = DEFAULT_SIDEBAR_HEIGHT;
            document.getElementById("col-lg-12").style.height = DEFAULT_COL_LG_12_HEIGHT;    
        }  
    }
    document.getElementById("sidebar-wrapper").style.height = sideBarWrapperHeight;
}      