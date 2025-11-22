Animator chỉ thiết lập [Idle] và [Blend tree: Moving [MovingUp], [MovingDown], [Running]]. Cơ chế game là một người đàn ông có thể đi trên tường, không ảnh hưởng trọng lực.

1. Ở trạng thái Idle, anh ấy có để đứng trên mặt đất, xoay mặt sang trái, phải (flip trục z), tương tự anh ấy có thể đứng trên tường trái, xoay mặt lên trên và xuống dưới (tương ứng với xoay trái và phải khi ở dưới đất), tương tự với khi Idle trên các mặt tường Trên và Phải

2. Ở trạng thái Idle, anh ấy có nhiều nhất 3 hướng di chuyển (trừ những hướng bị tường chặn, như khi đang ở mặt đất, không thể đi xuống dưới, hay đang ở góc trên bên phải, chỉ có thể đi xuống và trái). Logic về việc di chuyển đã được xử lý, chỉ cần xử lý phần animation

3. Những trường hợp từ Idle chuyển về Moving Up: -Từ mặt đất bay lên trần, -Từ tường trái bay sang phải và KHÔNG có mặt đất bên dưới chân, mà tương tự từ Phải sang Trái

4. Những trường hợp chuyển từ Idle về Running: Từ mặt đất di chuyển sang trái hoặc phải, Từ phải sang trái và trái sang phải TRONG TRƯỜNG HỢP CÓ MẶT ĐẤT NGAY DƯỚI CHÂN NGƯỜI ĐÓ. Ví dụ đang Idle trên trần, chân player chạm trần thì có thể running

5. Những trường hợp chuyển từ Idle về MovingDown: Tất cả trường hợp người đó di chuyển xuống dưới, _movingDirection = DOWN, kể cả từ trên trần, trái hoặc phải

6. Các trường hợp sau khi kết thúc Moving:

- Nếu đang running, chuyển về Idle với trạng thái ngược lại lastState, ví dụ đang chạy lại gặp tường, anh ấy Idle ngược lại hướng đã chạy

- Nếu đang Moving Up, Idle hướng nào cũng được

- Nếu đang MovingDown, nếu không có tường ở 2 bên thì Idle hướng nào cũng được, khi có tường thì Idle về bên không có tường
