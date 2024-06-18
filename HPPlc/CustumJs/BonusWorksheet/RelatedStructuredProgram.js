var InputWorksheet = {
	CultureInfo: $("#hdnCultureInfo").val(),
	selectedAgeGroup: $("#hdnAgeGroup").val(),
	selectedSubject: $("#hdnSubject").val(),
	worksheetId: $("#hdnWorksheet").val(),
	Mode: 'single'
};
$.post("/umbraco/Surface/StructuredProgram/GetSingleStructuredProgram",
	{
		InputWorksheet
	},
	function (data, status) {

		$("#WorksheetSingle").html("").append(data);

		//slicj4Function();
		share();
		$(".clsPrintDoc").click(function () {
			var vURLPath = $(this).find("span").html();

			try {
				var layerDetail = $(this).find('span').next('span').html();
				var pageDetail = $(this).find('span').next('span').next('span').html();

				printTracker(layerDetail, pageDetail);
			}
			catch (ex) {
				//cpnsole.log(ex);
			}

			PrintWorkSheet(vURLPath);

		});
		//var pdfValue = window.location.href;
		/*var pdfValue1 = pdfValue.split("?")[1];*/
		
		var pdfPreview = $("#PreviewPdf").val();
		WebViewer({
			path: 'lib', // path to the PDF.js Express'lib' folder on your server
			licenseKey: 'TnHM48Ax1AP5nVgJkq1N',
			//initialDoc: 'https://pdfjs-express.s3-us-west-2.amazonaws.com/docs/choosing-a-pdf-viewer.pdf',
			initialDoc: pdfPreview,
			disabledElements: [
				'viewControlsButton',
				'viewControlsOverlay',
				'leftPanelButton'
			]
			// initialDoc: '/path/to/my/file.pdf',  // You can also use documents on your server
		}, document.getElementById('viewer'))
			.then(instance => {
				// now you can access APIs through the WebViewer instance
				const { Core, UI } = instance;
				instance.UI.disableElements(['leftPanel', 'leftPanelButton']);
				instance.UI.disableElements(['leftPanel', 'menuButton']);

				instance.UI.disableElements(['leftPanel', 'searchButton']);

				instance.UI.disableFeatures([instance.UI.Feature.Print]);

				// adding an event listener for when a document is loaded
				Core.documentViewer.addEventListener('documentLoaded', () => {
					console.log('document loaded');
				});

				// adding an event listener for when the page number has changed
				Core.documentViewer.addEventListener('pageNumberUpdated', (pageNumber) => {
					console.log(`Page number is: ${pageNumber}`);
				});
			});
		
	});


var InputRelated = {

	CultureInfo: $("#hdnCultureInfo").val(),

	selectedAgeGroup: $("#hdnAgeGroup").val(),

	selectedSubject: $("#hdnSubject").val(),

	worksheetId: $("#hdnWorksheet").val(),

	Mode: 'relatedbytopic'

};

$.post("/umbraco/Surface/StructuredProgram/GetRelatedStructuredProgramDetails",

	{

		InputRelated

	},

	function (data, status) {

		$("#relatedworksheetbytopic").html("").append(data);

		$('#relatedworksheetbytopic .crslItm4').slick({

			dots: false,

			infinite: false,

			arrows: true,

			speed: 300,

			slidesToShow: 4,

			slidesToScroll: 4,

			responsive: [

				{

					breakpoint: 1199,

					settings: {

						arrows: false,

						slidesToShow: 3.1,

						slidesToScroll: 3

					}

				},

				{

					breakpoint: 767,

					settings: {

						arrows: false,

						slidesToShow: 2.3,

						slidesToScroll: 2

					}

				},

				{

					breakpoint: 480,

					settings: {

						arrows: false,

						slidesToShow: 2.1,

						slidesToScroll: 1

					}

				}

			]

		});

		slicj4Function();

		share();

		$(".clsPrintDoc").click(function () {

			var vURLPath = $(this).find("span").html();

			try {

				var layerDetail = $(this).find('span').next('span').html();

				var pageDetail = $(this).find('span').next('span').next('span').html();

				printTracker(layerDetail, pageDetail);

			}

			catch (ex) {

				//cpnsole.log(ex);

			}

			PrintWorkSheet(vURLPath);

		});

		$("#worksheetDetailsLoading").hide();

	});

InputRelated = {

	CultureInfo: $("#hdnCultureInfo").val(),

	selectedAgeGroup: $("#hdnAgeGroup").val(),

	selectedSubject: $("#hdnSubject").val(),

	worksheetId: $("#hdnWorksheet").val(),

	Mode: 'relatedbysubject'

};

$.post("/umbraco/Surface/StructuredProgram/GetRelatedStructuredProgramDetails",

	{

		InputRelated

	},

	function (data, status) {

		$("#relatedworksheetbysubject").html("").append(data);

		$('#relatedworksheetbysubject .crslItm4').slick({

			dots: false,

			infinite: false,

			arrows: true,

			speed: 300,

			slidesToShow: 4,

			slidesToScroll: 4,

			responsive: [

				{

					breakpoint: 1199,

					settings: {

						arrows: false,

						slidesToShow: 3.1,

						slidesToScroll: 3

					}

				},

				{

					breakpoint: 767,

					settings: {

						arrows: false,

						slidesToShow: 2.3,

						slidesToScroll: 2

					}

				},

				{

					breakpoint: 480,

					settings: {

						arrows: false,

						slidesToShow: 2.1,

						slidesToScroll: 1

					}

				}

			]

		});

		slicj4Function();

		share();

		$(".clsPrintDoc").click(function () {

			var vURLPath = $(this).find("span").html();

			try {

				var layerDetail = $(this).find('span').next('span').html();

				var pageDetail = $(this).find('span').next('span').next('span').html();

				printTracker(layerDetail, pageDetail);

			}

			catch (ex) {

				//cpnsole.log(ex);

			}

			PrintWorkSheet(vURLPath);

		});

		$("#worksheetDetailsLoading").hide();

	});
