# rostelecom-key-to-face
Проект позволяет добавить распознавания лиц для умного домофона от ростелеком. Настройка идентификации лиц производится с помощью телеграм бота - после каждой идентификации приходит сообщение с фотографией в котором можно указать запретить доступ или предоставить доступ этому лицу - это позволяет обучать Emgu.

![image](https://github.com/DrLenin/rostelecom-key-to-face/assets/68223185/5a6ffffc-dab0-4c7c-a225-5fa66a7f0e9f)

Чтобы приступить к работе:
1. Установите Google Chrome 113 версии - для получения картинки.
2. Укажите token бота в Config.
3. Укажите свой chatId в Config.
4. Укажите логин и пароль от ростелекома в Config.

Используются технологии: .Net Core + Emgu(идентификация лиц) + Selenium(для получения данных с видеокамеры)

--------

The project allows you to fasten face recognition for a smart intercom from Rostelecom. The identification of persons is set up using bot telegrams - after each identification, a message with a photo comes in which you can specify to deny access or grant access to this person - this allows you to train Emgu.

To get started:
1. Install Google Chrome 113 version - to get the picture.
2. Specify the bot token in Config.
3. Specify your chatId in Config.
4. Enter the login and password from Rostelecom in Config.

The following technologies are used: .Net Core + Emgu(Face identification) + Selenium (for receiving data from a video camera)

