if (typeof jQuery !== "undefined" && typeof saveAs !== "undefined") {
    (function($) {
        $.fn.wordExport = function(fileName) {
            fileName = typeof fileName !== 'undefined' ? fileName : "Procheckup-Report";
            var static = {
                mhtml: {
                    top: "Mime-Version: 1.0\nContent-Base: " + location.href + "\nContent-Type: Multipart/related; boundary=\"NEXT.ITEM-BOUNDARY\";type=\"text/html\"\n\n--NEXT.ITEM-BOUNDARY\nContent-Type: text/html; charset=\"utf-8\"\nContent-Location: " + location.href + "\n\n<!DOCTYPE html>\n<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"  xmlns:w=\"urn:schemas-microsoft-com:office:word\"  xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/TR/REC-html40\">\n_html_</html>",
                    head: "<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n<!--[if gte mso 9]><xml><w:WordDocument><w:Compatibility><w:DontGrowAutofit/></w:Compatibility><w:View>Print</w:View><w:Zoom>100</w:Zoom></w:WordDocument></xml><![endif]--><style>\n_styles_\n</style>\n</head>\n",
                    body: "<body lang=EN-US style='tab-interval:.5in'>_body_</body>"
                }
            };
            var options = {
                maxWidth: 624
            };
            // Clone selected element before manipulating it
            var markup = $(this).clone();

            // Remove hidden elements from the output
            markup.each(function() {
                var self = $(this);
                if (self.is(':hidden'))
                    self.remove();
            });

            // Embed all images using Data URLs
            var images = Array();
            var img = markup.find('img');
            for (var i = 0; i < img.length; i++) {
                
                // Calculate dimensions of output image
                var w = $(img[i]).css('width').replace('px', '').replace('pt', '');
                if (w == 15) {
                    w = 10;
                }
                var h = $(img[i]).css('height').replace('px', '').replace('pt', '');
                if (h == 15) {
                    h = 10;
                }
                //alert($('<canvas>').html());
                // Create canvas for converting image to data URL
                $('<canvas>').attr("id", "jQuery-Word-export_img_" + i).width(w).height(h).insertAfter(img[i]);
                var canvas = document.getElementById("jQuery-Word-export_img_" + i);
                
                canvas.width = w;
                canvas.height = h;
                console.log(w);
                // Draw image to canvas
                var context = canvas.getContext('2d');
                context.drawImage(img[i], 0, 0, w, h);
                // Get data URL encoding of image
                var uri = canvas.toDataURL();
                $(img[i]).attr("src", img[i].src);
                img[i].width = w;
                img[i].height = h;
                // Save encoded image to array
                images[i] = {
                    type: uri.substring(uri.indexOf(":") + 1, uri.indexOf(";")),
                    encoding: uri.substring(uri.indexOf(";") + 1, uri.indexOf(",")),
                    location: $(img[i]).attr("src"),
                    data: uri.substring(uri.indexOf(",") + 1)
                };
                // Remove canvas now that we no longer need it
                canvas.parentNode.removeChild(canvas);
            }

            // Prepare bottom of mhtml file with image data
            var mhtmlBottom = "\n";
            for (var i = 0; i < images.length; i++) {
                mhtmlBottom += "--NEXT.ITEM-BOUNDARY\n";
                mhtmlBottom += "Content-Location: " + images[i].contentLocation + "\n";
                mhtmlBottom += "Content-Type: " + images[i].contentType + "\n";
                mhtmlBottom += "Content-Transfer-Encoding: " + images[i].contentEncoding + "\n\n";
                mhtmlBottom += images[i].contentData + "\n\n";
            }
            mhtmlBottom += "--NEXT.ITEM-BOUNDARY--";

            //TODO: load css from included stylesheet
            var styles = "p.MsoFooter, li.MsoFooter, div.MsoFooter{margin:0in; margin-bottom:.0001pt; mso-pagination:widow-orphan;  tab-stops:center 3.0in right 6.0in; font-size:12.0pt;}</style><style><!-- /* Style Definitions */@page Section1{  size:841.9pt 595.3pt; margin:1.0in 1.0in 1.0in 1.0in;mso-page-orientation:landscape;  mso-header-margin:.5in;  mso-footer-margin:.5in;  mso-title-page:yes;  mso-header: h1;   mso-footer: f1;   mso-first-header: fh1;   mso-first-footer: ff1;  mso-paper-source:0;} div.Section1{ page:Section1; } table#hrdftrtbl{ margin:0in 0in 0in 900in; width:1px; height:1px; overflow:hidden;}--></style>";
            //styles = styles + '<style> #leaders {max-width: 40em; padding: 0; overflow-x: hidden; list-style: none} #leaders li:before { float: left; width: 0; white-space: nowrap; content:". . . . . . . . . . . . . . . . . . . . "  ". . . . . . . . . . . . . . . . . . . . " ". . . . . . . . . . . . . . . . . . . . " ". . . . . . . . . . . . . . . . . . . . "} #leaders span:first-child { padding-right: 0.33em; background: white} #leaders span + span { float: right; padding-left: 0.33em; background: white}</style>';
            styles = styles + '<style> ul.dotted li, ul.dotted div { height: inherit;  list-style-type: none; } ul.dotted > li div { float: left;  width: 50%; position: relative; overflow: hidden; } .dotted div.item { width: 80%; } .dotted div.prices { width: 20%;  overflow: visible; } .dotted div.item, .dotted div.prices { height: 1em; font-weight: bold; } ul.dotted span { display: block;  position: absolute; padding: 0 5px; } ul.dotted span:after { content: "";  display: block; position: absolute;  top: 67%; width: 1000px; border-top: 2px dotted #000; } div.item span { left: 0;} div.item span:after { left: 100%; } div.prices span {  right: 0;  width: 80%;  }  div.prices div { width: 100% !important; }   div.prices span:after { right: 100%;}  ul.dotted p { padding: 0px 10px 0px; clear: both; margin-bottom: 0; }</style>';
            var fileContent = static.mhtml.top.replace("_html_", static.mhtml.head.replace("_styles_", styles) + static.mhtml.body.replace("_body_", markup.html())) + mhtmlBottom;
            //console.log(styles);
            // Create a Blob with the file contents
            var contents = "<html>\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n<!--[if gte mso 9]><xml><w:WordDocument><w:View>Print</w:View><w:Zoom>100</w:Zoom></w:WordDocument></xml><![endif]--><style>\n" + styles + "\n</style>\n</head>\n<body lang=EN-US style='tab-interval:.5in'>" + markup.html() + "</body></html>";
            
            //$.ajax({
            //    type: "POST",
            //    url: "/Customers/wordDownload",
            //    dataType: "json",
            //    async: false,
            //    timeout: 3000,
            //    beforeSend: function () {

            //    },
            //    success: function (msg) {
            //        console.log(msg);

            //        if (msg) {

            //            id = msg;

            //        }
            //        else {

            //        }
            //    },
            //    failure: function (msg) {
            //        console.log(msg);

            //    },
            //    data: { 'data': contents},
               
            //});
            var blob = new Blob([fileContent], {
                type: "application/msword;charset=utf-8"
            });
            saveAs(blob, fileName + ".doc");
            
        };
    })(jQuery);
} else {
    if (typeof jQuery === "undefined") {
        console.error("jQuery Word Export: missing dependency (jQuery)");
    }
    if (typeof saveAs === "undefined") {
        console.error("jQuery Word Export: missing dependency (FileSaver.js)");
    };
}
