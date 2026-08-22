import "styles/components/page-transition.css";

import { AnimatePresence, motion, type Variants } from "framer-motion";
import React from "react";
import { type Location, useLocation } from "react-router-dom";

interface Props {
  readonly children: React.ReactNode;
  readonly className?: string;
}

const variants: Variants = {
  initial: {
    y: 0,
    scale: 0.5,
    opacity: 0
  },
  animate: {
    y: 0,
    scale: 1,
    opacity: 1,
    transition: {
      duration: 0.6,
      ease: [0.22, 1, 0.36, 1]
    }
  },
  exit: {}
};

export const PageTransition: React.FC<Props> = ({ children, className }: Props) => {
  const location: Location = useLocation();

  return (
    <AnimatePresence mode="wait">
      <motion.div
        key={location.pathname}
        className={`transition-main ${className}`}
        initial="initial"
        animate="animate"
        exit="exit"
        variants={variants}>
        {children}
      </motion.div>
    </AnimatePresence>
  );
};
