import "styles/components/yandex-map.css";

import React, { type RefObject, useEffect, useRef } from "react";
import { useTranslation } from "react-i18next";
import { currentLanguage } from "utils";

interface Props {
  readonly address: string;
  readonly className?: string;
}

export const YandexMap: React.FC<Props> = ({ address, className }: Props) => {
  const { i18n } = useTranslation();

  const ymapsScriptId: string = "ymaps-api-script";
  const mapContainerRef: RefObject<HTMLDivElement | null> = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    let isMounted: boolean = true;

    /* eslint-disable */
    const loadScript = () => {
      if (document.getElementById(ymapsScriptId)) {
        return Promise.resolve();
      }

      const language = currentLanguage(i18n);

      return new Promise<void>((resolve, reject) => {
        const script = document.createElement("script");
        script.id = ymapsScriptId;
        script.src = `https://api-maps.yandex.ru/2.1/?lang=${language}`;
        script.async = true;
        script.onload = () => resolve();
        script.onerror = () => reject(new Error("Yandex Maps API load error"));

        document.head.appendChild(script);
      });
    };

    const initMap = async () => {
      try {
        await loadScript();

        const ymaps: any = (window as any).ymaps;

        if (ymaps) {
          await ymaps.ready();
        }

        if (!isMounted || !mapContainerRef.current) {
          return;
        }

        const map: any = new ymaps.Map(mapContainerRef.current, {
          zoom: 10,
          center: [55.76, 37.64],
          controls: ["zoomControl", "typeSelector", "fullscreenControl"]
        });

        const geocodeResult = await ymaps.geocode(address);
        const firstGeoObject = geocodeResult.geoObjects.get(0);

        if (firstGeoObject) {
          const coordinates = firstGeoObject.geometry.getCoordinates();
          map.setCenter(coordinates, 14);

          const placemark = new ymaps.Placemark(coordinates, {
            balloonContent: firstGeoObject.getAddressLine() || address
          });

          map.geoObjects.add(placemark);
        }
      } catch (e) {
        console.error(e);
      }
    };
    /* eslint-enable */

    initMap();

    return () => {
      isMounted = false;

      if (mapContainerRef.current) {
        mapContainerRef.current.innerHTML = "";
      }
    };
  }, [address]);

  return (
    <div className={className}>
      <div className="yandex-map-component" role="region" ref={mapContainerRef} />
    </div>
  );
};
