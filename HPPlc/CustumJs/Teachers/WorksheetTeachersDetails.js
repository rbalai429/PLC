
var vrSubjectGroupe = '';
var vrTopicsGroupe = '';
var vrCBSEContent = false;

vrSubjectGroupe = $(".subjects:checked").map(function () { return this.value }).get().join(", ");
vrTopicsGroupe = $(".topics:checked").map(function () { return this.value }).get().join(", ");

var Input = {
	CultureInfo: $("#hdnCultureInfo").val(),
	FilterId: $("#hdnqtype").val()
};
$.post("/umbraco/Surface/Teachers/GetWorkSheetById",
	{
		Input
	},
	function (data, status) {
		
		$("#WorksheetSingle").html("").append(data);

		share();
		//SelectedParameters();
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
		
		//var bLazy = new Blazy();
		//var bLazy = new Blazy({
		//	selector: '.b-lazy',
		//	loadInvisible: true,
		//	offset: 200
		//});

		//var imagesToLoad = document.querySelectorAll('.b-lazy');
		//bLazy.revalidate();
		//bLazy.load(imagesToLoad);

		$("#worksheetDetailsLoading").hide();

		
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
