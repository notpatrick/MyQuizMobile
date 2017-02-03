# MyQuizMobile

#### This repository contains a project for one of my university courses.


### Description

The application represents an alternative implementation to combine the [**MyQuizSupervisor**](https://github.com/TitanNano/MYQuizSupervisor) and [**MyQuizAdmin**](https://github.com/DerTieran/MyQuizAdmin) roles of the MyQuiz project. It communicates with the [**MyQuizBackend**](https://github.com/Kulu-M/MyQuizBackend) to create, send and supervise votes which are then sent to [**MyQuizClients**](https://github.com/TitanNano/MYQuizClient). Managing all relevant data contained in the DataModel like Groups, SingleTopics, QuestionBlocks, Questions and AnswerOptions should also possible. It is planned to implement a comprehensive UI to show visual statistics of previous votes.
 
 For the time being, only **_Android_** is a target plattform until all functionality is implemented to avoid the necessity of creating custom, platformspecific renderers.
 
 ### Technologies
 * C#
 * Xamarin.Forms
 * PostSharp
 * NewtonSoft.Json
 * ModernHttpClient

### Milestones

- [ ] Full vote capabilities
- [ ] Full data management capabilities
- [ ] Full statistics capabilities