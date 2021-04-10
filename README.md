# Detailing Arsenal - Buffing Pad Compare Tool for Auto Detailers

Live at: https://detailingarsenal.com

## Description

Detailing Arsenal is an interactive website that allows users to compare buffing pads of multiple manufacturers, and leave reviews based on their experiences.

When it comes to picking out buffing pads, detailers are faced with an overwhelming amount of options. There's a lot of factors that need to be considered such as pad aggressiveness, pad size, material, and more. Currently, the best way to find out what pads you should buy is to spend some time reading up on the forums and see what others recommend. While there's nothing wrong with that, it can be extremely time consuming.

## Implementation

The web app is divided into 3 parts:

-   Vue.js Single Page Application (frontend)
-   ASP.NET Core Web API (backend)
-   PostgreSQL database (data persistence)

### Frontend

The website itself is a Vue.js single page application, and can be found under `/app`. It's written in TypeScript and uses Vue 2. The frontend uses Bulma.io for basic styling, and Buefy for components. The project itself has some additional CSS helpers I've written in SASS that can be found in `./app/src/assets/styles`.

VeeValidate is used for validation, and state management is handled thanks to Vuex.

To help simplify the project structure and isolate different contexts from each other I've broken the project up into feature modules.

The website is designed to be responsive thanks to Bulma's responsive helpers and looks good on any screen size whether it be a phone, tablet, or laptop.

```
./src
    /api --API client services for the backend
    /core --Boilerplate code such as generic components, and more used by the entire project
        /modules
        /admin --Admin panel for managing the site
        /product-catalog --The buffing pad catalog
        /scheduling --Legacy portion of the app as it used to be a scheduling app
        /shared --Specialized code shared between modules
        /user --User control panels such as profile page, etc...
```

### Backend

The backend is where the real meat of the project lies. It's written in C# and uses ASP.NET Core Web API. It's a RESTful API that uses Domain Driven Design along with Command and Query Responsibility Separation. Dapper is used as a MicroORM, and Auth0 is used for authentication. Authorization is done via Role Based Access Control.

It's divided into 6 layers and each layer has feature modules like the frontend.

```
./src
    /Api --API controllers for handling HTTP requests, and responses
    /Application --CQRS layer to handle validation, coordinating, authorization, and authentication
    /Domain --Business logic lies here. Includes all of the domain models and more
    /Infrastructure --3rd party integrations that are abstracted behind interfaces
    /Persistence --Database interactions such as repositories, and migrations
    /Shared --Utilities, and more that is used by the entire backend.
./tests -- Unit / integration tests
```

Testing is done via Microsoft's unit testing framework (mstest).

## Deployment

The API is deployed on a DigitalOcean droplet, and features a CI/CD pipeline thanks to Github Actions.

The frontend is deployed with Netlify and also includes a CI/CD pipeline.

The database is running on DigitalOCean

## Post Morten

While the project is alive, and in deployment it's currently in a dormant state with no active development taking place. When I started this project Vue 3 was still in beta and I opted to stick with Vue 2 as it was what I was most familiar with at the time. Now with the release of Vue 3 it's basically turned the entire project into legacy code as it'll require a full rewrite to be migrate.

As of right now there's no plans to continue on with development but that may change.
