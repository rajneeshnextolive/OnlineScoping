    $(document).ready(function () {
        $("#finish").hide();
            $("#3b1").show();
            var carouselLength = $('.carousel-inner .carousel-item').length - 1;
            if (carouselLength == -1) {
        $("#finish").show();
                $("#Next").hide();
            }

            if (carouselLength == 0) {
        $("#finish").show();
                $("#Next").hide();
            }
            $('#carouselExampleControls').on('slid.bs.carousel', function (e) {
                if ($('.carousel-inner .carousel-item:last').hasClass('active')) {
        $("#finish").show();
                    $("#Next").hide();
                }
            });

            $("#dbe5c019-2098-4a3b-bf2a-4c9ea35c3584").click(function () {
                var rVal2 = $(" #dbe5c019-2098-4a3b-bf2a-4c9ea35c3584:checked ").val();
                if (rVal2 == "dbe5c019-2098-4a3b-bf2a-4c9ea35c3584") {
        $("#3b1").show();
                }
            });

        $("#38107f92-f2c9-4e22-8ab6-87cd0f5a1fdc").click(function () {
                var rVal2 = $(" #38107f92-f2c9-4e22-8ab6-87cd0f5a1fdc:checked ").val();
                if (rVal2 == "38107f92-f2c9-4e22-8ab6-87cd0f5a1fdc") {
        $("#3b1").hide();
                }
            });


        $("#Next").click(function () {
            $('.carousel-inner .carousel-item.active').find('input').each(function () {
                var rVal = $(".carousel-inner .carousel-item.active #03503f7b-2c04-4c44-8690-541b8fa6366a:checked ").val();
                if (rVal == "03503f7b-2c04-4c44-8690-541b8fa6366a") {
                    $('#exampleModal').modal('show');
                }
                var rVal7a = $(".carousel-inner .carousel-item.active #c56f31c4-d52e-4942-b16e-3046cfa67843:checked ").val();
                if (rVal7a == "c56f31c4-d52e-4942-b16e-3046cfa67843") {
                    $('#exampleModalMsg3').modal('show');
                }

            })

            var f7 = $('#7f').hasClass('active');
            if (f7 == true) {
                $('#ModalMsg7f').modal('show');
            }


            var a8 = $(" #503c9f04-9620-484e-adc3-64507c2f7e8d:checked ").val();
            if (a8 == "503c9f04-9620-484e-adc3-64507c2f7e8d") {
                $('#exampleModalMsg8').modal('show');
            }
        });

            $("#less").click(function () {
        $('#exampleModalMsg').modal('show');
            })
            $("#more").click(function () {
        $(".carouselExampleControls").carousel("next")
    })

            flatpickr(".calendar", {
        defaultDate:'today'
            });

        });