import React, { ReactNode, useRef } from "react";
import styles from "./ModalDialog.module.css";

type ModalDialogProps = {
  title: string;
  onOk?: () => void;
  onCancel?: () => void;
  children: ReactNode;
}

export default function ModalDialog({ title, onOk, onCancel, children }: ModalDialogProps) {
  const modalRef = useRef<HTMLDivElement>(null);

  const handleResize = (event: React.MouseEvent) => {
    event.preventDefault();

    const modal = modalRef.current;
    if (!modal) return;

    const startMouseX = event.clientX;
    const startMouseY = event.clientY;
    const startWidth = modal.offsetWidth;
    const startHeight = modal.offsetHeight;

    const onMouseMove = (e: MouseEvent) => {
      const deltaX = e.clientX - startMouseX;
      const deltaY = e.clientY - startMouseY;

      // Dialog ist zentriert, daher 2x deltaX
      modal.style.width = `${Math.max(640, startWidth + 2 * deltaX)}px`;
      modal.style.height = `${Math.max(480, startHeight + 2 * deltaY)}px`;
    };

    const onMouseUp = () => {
      document.removeEventListener("mousemove", onMouseMove);
      document.removeEventListener("mouseup", onMouseUp);
    };

    document.addEventListener("mousemove", onMouseMove);
    document.addEventListener("mouseup", onMouseUp);
  };

  return (
    <div className={styles.overlay}>
      <div className={styles.modal} ref={modalRef}>
        <div className={styles.header}>
          <h2>{title}</h2>
        </div>
        <div className={styles.content}>{children}</div>
        <div className={styles.footer}>
          <button className={styles.cancelButton} onClick={onCancel}>
            Cancel
          </button>
          <button className={styles.okButton} onClick={onOk}>
            OK
          </button>
        </div>
        <div
          className={styles.resizeHandle}
          onMouseDown={handleResize}
          aria-label="Resize Handle"
        ></div>
      </div>
    </div>
  );
}
