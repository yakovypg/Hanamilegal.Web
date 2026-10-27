import { type ViewPanel } from "types";

export const getRoutePath = (view: ViewPanel): string => {
  return `/${view}`;
};

export const getRoutePathWithSlug = (view: ViewPanel): string => {
  return `${getRoutePath(view)}/:slug`;
};
