# API

The API subfolder is responsible for housing the API client. It'll hold all of the data transfer objects (DTOs) and the services that comprise the API.

The frontend is divided into several different modules, and at first the API was split across each of these modules. Over time though, it became clear this was problematic as some API services need to be used in more than 1 module. This created an ugly web of interdepedencies between modules when each one should be self sustaining.

# How To Use

To access any service use the `api` singleton.

```
// Bad
var userService = new UserService();

// Good
var user = api.user.getUser()
```
