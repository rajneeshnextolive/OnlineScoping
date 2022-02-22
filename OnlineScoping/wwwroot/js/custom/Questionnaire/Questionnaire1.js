
$(document).ready(function () {
    //External Infrastructure Scoping Questionnaire
    $("#8a").show();
    $("#12a").show();
    $("#6ay").show();
    $("#04c742d9-6c7a-48b5-9cfb-5959476c9032").click(function () {
        var rVal2 = $("#04c742d9-6c7a-48b5-9cfb-5959476c9032:checked ").val();
        if (rVal2 == "04c742d9-6c7a-48b5-9cfb-5959476c9032") {
            $("#8a").show();
        }
    });

    $("#c445f578-bdac-4f3e-9f66-63b19dfcb6d7").click(function () {
        var rVal2 = $("#c445f578-bdac-4f3e-9f66-63b19dfcb6d7:checked ").val();
        if (rVal2 == "c445f578-bdac-4f3e-9f66-63b19dfcb6d7") {
            $("#8a").hide();
        }
    });

    //Internal Infrastructure Scoping Questionnaire
    
    $("#d6557126-e6dc-43b2-b66d-2b28462baae6").click(function () {
        var rVal2 = $("#d6557126-e6dc-43b2-b66d-2b28462baae6:checked ").val();
        if (rVal2 == "d6557126-e6dc-43b2-b66d-2b28462baae6") {
            $("#11a").show();
        }
    });

    $("#4a9d91f3-4efb-42a9-ba13-ec60abac78ff").click(function () {
        var rVal2 = $("#4a9d91f3-4efb-42a9-ba13-ec60abac78ff:checked ").val();
        if (rVal2 == "4a9d91f3-4efb-42a9-ba13-ec60abac78ff") {
            $("#11a").hide();
        }
    });
    //=======================================
    $("#b462f7b9-cd01-46ab-8176-1f7fcdcb2339").click(function () {
        var rVal2 = $("#b462f7b9-cd01-46ab-8176-1f7fcdcb2339:checked ").val();
        if (rVal2 == "b462f7b9-cd01-46ab-8176-1f7fcdcb2339") {
            $("#12a").show();
        }
    });

    $("#224558d2-145d-43e4-9375-d1b458610962").click(function () {
        var rVal2 = $("#224558d2-145d-43e4-9375-d1b458610962:checked ").val();
        if (rVal2 == "224558d2-145d-43e4-9375-d1b458610962") {
            $("#12a").hide();
        }
    });
    //======================================================

   
    $("#8acb132a-e5ba-4c62-84ea-80fe780c1923").click(function () {
       
        var rVal2 = $("#8acb132a-e5ba-4c62-84ea-80fe780c1923:checked ").val();
        if (rVal2 == "8acb132a-e5ba-4c62-84ea-80fe780c1923") {
            $("#6ay").show();
        }
    });

    $("#501d4cd5-8c21-4a6d-9704-d5899c740d5b").click(function () {
        
        var rVal2 = $("#501d4cd5-8c21-4a6d-9704-d5899c740d5b:checked ").val();

        if (rVal2 == "501d4cd5-8c21-4a6d-9704-d5899c740d5b") {
            $("#6ay").hide();
        }
    });
     //Cyber Essentials Plus Assessment

    $("#d13ca5e1-439f-4247-9dd1-362b64a8d77e").click(function () {
        var rVal2 = $("#d13ca5e1-439f-4247-9dd1-362b64a8d77e:checked ").val();
        if (rVal2 == "d13ca5e1-439f-4247-9dd1-362b64a8d77e") {
            $("#13").show();
        }
    });

    $("#a559e534-50fe-4995-ba47-fdda54285d68").click(function () {     
        var rVal2 = $("#a559e534-50fe-4995-ba47-fdda54285d68:checked ").val();

        if (rVal2 == "a559e534-50fe-4995-ba47-fdda54285d68") {
            $("#13").hide();
        }
    });
    //office 365
    $("#739beb8f-bbcc-4aca-bc08-4e005fb4805b").click(function () {
        var rVal2 = $("#739beb8f-bbcc-4aca-bc08-4e005fb4805b:checked ").val();
        if (rVal2 == "739beb8f-bbcc-4aca-bc08-4e005fb4805b") {
            $("#15a").show();
        }
    });

    $("#f07206f8-dec2-4f10-bc9f-e7118cf67890").click(function () {
        var rVal2 = $("#f07206f8-dec2-4f10-bc9f-e7118cf67890:checked ").val();

        if (rVal2 == "f07206f8-dec2-4f10-bc9f-e7118cf67890") {
            $("#15a").hide();
        }
    });


    $("#a9e7f6f8-10fd-4010-9d23-1e6727d20343").click(function () {
        var rVal2 = $("#a9e7f6f8-10fd-4010-9d23-1e6727d20343:checked ").val();
        if (rVal2 == "a9e7f6f8-10fd-4010-9d23-1e6727d20343") {
            $("#21a").show();
            $("#21b").show();
        }
    });

    $("#9ef2cad4-6ae8-4f13-9cae-c9ace1d5af50").click(function () {
        var rVal2 = $("#9ef2cad4-6ae8-4f13-9cae-c9ace1d5af50:checked ").val();

        if (rVal2 == "9ef2cad4-6ae8-4f13-9cae-c9ace1d5af50") {
            $("#21a").hide();
            $("#21b").hide();
        }
    });







    flatpickr(".calendar", {
        defaultDate: 'today'
    });

});