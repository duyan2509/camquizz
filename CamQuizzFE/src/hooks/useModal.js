import { useState, useCallback } from 'react';

export const useModal = (handleSubmit, handleClose) => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    const showModal = useCallback(() => {
        setIsModalOpen(true);
    }, []);


    const handleOk = useCallback(async () => {
        if (!handleSubmit) {
            setIsModalOpen(false);
            return;
        }

        try {
            const result = await handleSubmit();
            console.log("a",result)
            if (result) {
                setIsModalOpen(false);
            }
        } catch (error) {
            console.error("Error in handleSubmit:", error);
        }
    }, [handleSubmit]);

    const handleCancel = useCallback(() => {
        handleClose?.();
        setIsModalOpen(false);
    }, [handleClose]);

    return {
        isModalOpen,
        showModal,
        handleOk,
        handleCancel,
    };
};
