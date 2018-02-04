const DEFAULT_SIDEBAR_HEIGHT = '1000px';


//  Expand Side bar height after search
document.addEventListener("DOMContentLoaded", function () { windowLoaded() });
function windowLoaded() {
   
    var sideBarWrapperHeight;
    if (document.getElementById("page-content-wrapper") === null)
    {
        document.getElementById("sidebar-wrapper").style.height = DEFAULT_SIDEBAR_HEIGHT;
    }
    else
    {
        sideBarWrapperHeight = document.getElementsByClassName("page-content-wrapper").height;
        if (sideBarWrapperHeight < DEFAULT_SIDEBAR_HEIGHT)
        {
            sideBarWrapperHeight = DEFAULT_SIDEBAR_HEIGHT;
            document.getElementById("container-fluid").style.height = '900 px';
        }
    }
    document.getElementById("sidebar-wrapper").style.height = sideBarWrapperHeight;
}      