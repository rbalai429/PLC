var disabledate = ["1/2/2022", "3/2/2022"];//dates to disable

var today = new Date();
var tomorrow = new Date();
var maxday = new Date();


tomorrow.setDate(today.getDate() + 1);
// maxday.setDate(today.getDate()+7+disabledate.length);

const numToAdd = 6;

for (let i = 1; i <= numToAdd; i++) {
    maxday.setDate(maxday.getDate() + 1);
    if (maxday.getDay() === 6) {
        maxday.setDate(maxday.getDate() + 2);
    }
    else if (maxday.getDay() === 0) {
        maxday.setDate(maxday.getDate() + 1);
    }
}

// console.log(now+"===="+ today)

$('#datepiker').datepicker({

    startDate: tomorrow,
    format: 'dd-mm-yyyy',
    endDate: maxday,
    filter: function (date, view) {

        if (date.getDay() === 0 && view === 'day') {
            return false; // Disable all Sundays, but still leave months/years, whose first day is a Sunday, enabled.
        }

        $thisDate = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();
        console.log($thisDate);
        if ($.inArray($thisDate, disabledate) != -1 && date.getDay() != 0) {
            return false;
        }
    }
});
