function tfBookingGridHelloWorld() {
    alert("hw");
}

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
            .fail(function(xhr) {
                var json = xhr.responseJSON ? JSON.parse(xhr.responseJSON) : null;
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
            function (success, response) {
                console.log("Success:", success);
                console.log("Response:", response);
            }
        )
    }

}