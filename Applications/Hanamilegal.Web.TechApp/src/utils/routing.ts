import { type ServiceSlug, VIEW_PANELS, type ViewPanel } from "types";

export const getRoutePath = (view: ViewPanel): string => {
  return `/${view}`;
};

export const getRoutePathWithSlug = (view: ViewPanel): string => {
  return `${getRoutePath(view)}/:slug`;
};

export const getServiceRoutePath = (slug: ServiceSlug): string => {
  return `${getRoutePath(VIEW_PANELS.services)}/${slug}`;
};
