import { ServiceDetailsPage } from "pages";
import React from "react";
import { useTranslation } from "react-i18next";
import { Navigate, useParams } from "react-router-dom";
import { SERVICE_CONFIGS, type ServiceConfig, type ServiceSlug, VIEW_PANELS } from "types";
import { getRoutePath } from "utils";

export const ServiceDetailWrapper: React.FC = () => {
  const { slug } = useParams<{ slug: string }>();
  const { t } = useTranslation();

  if (!slug) {
    return <Navigate to={getRoutePath(VIEW_PANELS.notFound)} replace />;
  }

  const serviceSlug: ServiceSlug = slug as ServiceSlug;
  const serviceConfig: ServiceConfig = SERVICE_CONFIGS[serviceSlug];
  const serviceName: string = t(`service.${serviceConfig.translationName}.name`);
  const serviceDescription: string = t(`service.${serviceConfig.translationName}.description`);

  return (
    <ServiceDetailsPage
      name={serviceName}
      description={serviceDescription}
      exampleImageSource={serviceConfig.exampleImageSource}
    />
  );
};
