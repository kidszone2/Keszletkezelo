/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class TestFlightBookingProxy {

    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    invoke(method, url, data, callback) {
        $.ajax({
            url: url,
            type: method,
            data: data,
            cache: false,
            success: function(response) {
                callback(true, response);
            }
        })
            .fail(function (xhr) {
                var json = xhr.responseJSON ? xhr.responseJSON : null;
                var jsonError = json && json.error ? json.error : null;

                var message = jsonError
                    || `Request from ${url} failed with status: ${xhr.status}`;

                callback(false, message);
            });
    }

    post(url, data, callback) {
        this.invoke('POST', url, data, callback);
    }

    create(departureAt, planID, callback) {
        this.post(
            this.baseUrl + 'Booking/Create',
            {
                DepartureAt: departureAt,
                PlanID: planID
            },
            callback
        )
    }

}
/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingGridPassengerRow {

    constructor($row) {
        this.changedCallback = null;
        this.$row = $row;
        this.attach();
        this.refresh();
    }

    attach() {
        this.$type = this.$row.find('select[name="PassengerRole"]');
        this.$passenger = this.$row.find('input[name="PassengerName"]');
        this.$license = this.$row.find('input[name="PilotLicense"]');

        var that = this;
        this.$type.change(function (e) { that.onTypeChanged(); })
    }

    refresh() {
        var type = this.$type.val();
        this.$passenger.prop('disabled', !type);
        this.$license.prop('disabled', type != 'pilot');
    }

    setVisiblity(visible) {
        this.$row.visible(visible);

    }

    onTypeChanged() {
        this.refresh();

        if (this.changedCallback) {
            this.changedCallback();
        }
    }
}



/******************************************************************************
 *
 *   Copyright Corvinus University of Budapest, Budapest, HUN.
 *
 * ---------------------------------------------------------------------------
 *   This is a part of the RF.Modules.TestFlightAppointment project
 *
 *****************************************************************************/

class BookingGridForm {
    
    constructor (selector) {
        this.$grid = $(selector);
        this.attach();
    }

    attach() {
        var that = this;

        that.rows = [];        
        this.$grid.find('[data-role="tf-passenger-row"]')
            .each(function(idx, element) {
                var row = new BookingGridPassengerRow($(element));
                that.rows.push(row);
            });
    }
}