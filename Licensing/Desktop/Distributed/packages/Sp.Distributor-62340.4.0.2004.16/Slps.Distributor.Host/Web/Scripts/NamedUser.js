$(document).ready(function () {
    var availableDurations;
    var namedUsers;
    $('#assign').click(function (e) {
        var assign = window.location.href.replace("For", "Assign");
        var usernameList = $('#username').val();
        var duration = $('#duration option:selected').val();
        var available = availableDurations.availability.filter(function (item) { return item.LockDuration == duration }).map(function (item) { return item.Available })[0];
        var assignments = $('#username').val().split(';');
        if (assignments.length > available) {
            setAssignMessage("Insufficient availability remaining to assign " + assignments.length + " user(s) to the duration " + duration + " day(s).", true);
            return;
        }
        var nonUniqueUsernames = _.intersection(assignments, namedUsers.users.map(function (item) { return item.Username }));
        if (nonUniqueUsernames.length > 0)
        {
            setAssignMessage("Cannot assign users. Usernames must be unique. The user(s) " + nonUniqueUsernames.join(', ') + " already exist", true);
            return;

        }
        $.post(assign, { username: usernameList, days: duration })
            .success(function (data) {
                $('#username').val('');
                setAssignMessage("Successfully assigned user(s).", false);
                load();
            })
            .error(function (error) {
                setAssignMessage(error.responseText, true);
            });
    });

    getAvailability = function () {
        var availability = window.location.href.replace("For", "Get");

        $.get(availability).success(function (data) {
            availableDurations = { availability: data };
            var availabilityTemplate = $('#availability-template').html();
            var availabilityRows = Mustache.render(availabilityTemplate, availableDurations);
            $('#availabilityTable').html(availabilityRows);

            var durations = { durations: data.filter(function (item) { return item.Available > 0 }).map(function (item) { return { LockDuration: item.LockDuration } }) };
            var durationsTemplate = $('#duration-template').html();
            var durationOptions = Mustache.render(durationsTemplate, durations);
            $('#duration').html(durationOptions);
            $('#assign').attr("disabled", durations.durations.length == 0);
        });
    }

    getNamedUsers = function () {
        var namedUsersUrl = window.location.href.replace("For", "NamedUserIndex");

        $.get(namedUsersUrl).success(function (data) {
            var users = _.sortBy(JSON.parse(data), 'UserName');
            
            namedUsers = {
                users: users.map(function (item) {
                    return { Username: item.UserName, LockDuration: item.LockDuration, Expiry: moment(item.Expiry).format('MMMM Do YYYY'), unlocked: new Date(item.Expiry) < Date.now() }
                })
            };
            var namedUsersTemplate = $('#namedUsers-template').html();
            var userRows = Mustache.render(namedUsersTemplate, namedUsers);
            $('#namedUsersTable').html(userRows);
            wireUnassignmentEvent();
        }).error(function (response) {
            console.log(response.responseText);
        });
    }

    load = function () {
        getAvailability();
        getNamedUsers();
    }

    wireUnassignmentEvent = function () {
        $("button.unassign").click(function (evt) {
            var username = $(evt.target).closest("tr").find("td.username").text().trim();
            var root = window.location.href.replace("For", "Unassign");
            $.post(root, { username: username })
                  .success(function (data) {
                      load();
                  })
                .error(function (error) {
                    setUnassignError(error.responseText);
                });
        });
    }

    setAssignMessage = function (message, error) {
        $('#assignMessage').text(message);
        if (error)
        {
            $('#assignMessage').css('color', 'red');
        }
        else
        {
            $('#assignMessage').css('color', 'green');
        }
        $('#assignMessage').show().delay(6000).fadeOut('slow');
    }

    setUnassignError = function (errorMessage) {
        $('#unassignErrorMessage').text(errorMessage);
        $('#unassignErrorMessage').show().delay(6000).fadeOut('slow');
    }

    load();
});
