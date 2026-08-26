import "styles/pages/reviews-page.css";

import { Review } from "components";
import React from "react";
import { useTranslation } from "react-i18next";

import avatar from "/example-desktop.png";

export const ReviewsPage: React.FC = () => {
  const { t } = useTranslation();

  return (
    <div>
      <h3 className="text-center text-decoration-underline">{t("title.reviews")}</h3>
      <div className="mt-5 reviews-row">
        <Review
          avatarImageSource={avatar}
          author="ИП Иванов Иван Иванович"
          text="Огромное спасибо команме за Desktop‑приложение — оно работает быстро и стабильно, интерфейс интуитивный, а миграция данных прошла без проблем; это значительно упростило нашу повседневную работу."
        />
        <Review
          avatarImageSource={avatar}
          author="ООО Компания"
          text="Благодарим за разработку Web‑приложения: современный дизайн, чёткая архитектура и высокая производительность; пользователи в восторге, а мы получили все функции в оговоренные сроки."
        />
        <Review
          avatarImageSource={avatar}
          author="АО Родная Нефть"
          text="Спасибо за CLI‑утилиту — она автоматизировала рутинные задачи, проста в использовании и легко интегрируется в наши скрипты; команда сэкономила много времени и ошибок."
        />
        <Review
          avatarImageSource={avatar}
          author="Матвей Тимофеевич"
          text="Большое спасибо за библиотеку классов на C# — код аккуратный и понятный, документация исчерпывающая, интеграция в наш проект прошла безболезненно; теперь разработка фич идёт гораздо быстрее."
        />
      </div>
    </div>
  );
};
